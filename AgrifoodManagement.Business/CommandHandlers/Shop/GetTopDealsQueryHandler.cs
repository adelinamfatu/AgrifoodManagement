using AgrifoodManagement.Business.Queries.Shop;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Shop
{
    public class GetTopDealsQueryHandler : IRequestHandler<GetTopDealsQuery, Result<List<ProductDto>>>
    {
        private readonly ApplicationDbContext _context;

        public GetTopDealsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ProductDto>>> Handle(GetTopDealsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var now = DateTime.UtcNow.Date;

                var discounted = await _context.Products
                    .AsNoTracking()
                    .Where(p => p.CurrentPrice != p.OriginalPrice && p.CurrentPrice != null
                        && p.AnnouncementStatus == AnnouncementStatus.Published
                        && p.ExpirationDate.Date >= now)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.CurrentPrice,
                        p.OriginalPrice,
                        p.UnitOfMeasurement,
                        p.ExpirationDate,
                        p.Quantity,
                        Discount = (p.OriginalPrice - p.CurrentPrice) / p.OriginalPrice * 100
                    })
                    .OrderBy(x => x.ExpirationDate)
                    .Take(request.TakeCount)
                    .ToListAsync(cancellationToken);

                var productIds = discounted.Select(x => x.Id).ToList();
                var photoUrlsDict = await _context.ExtendedProperties
                    .Where(ep => ep.Key == "PhotoUrl")
                    .GroupBy(ep => ep.EntityId)
                    .ToDictionaryAsync(g => g.Key, g => new List<string>
                        {
                            g.Select(x => x.Value).FirstOrDefault()
                        }, cancellationToken);

                var dtos = discounted.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CurrentPrice = (decimal)p.CurrentPrice,
                    OriginalPrice = p.OriginalPrice,
                    UnitOfMeasurement = p.UnitOfMeasurement,
                    ExpirationDate = p.ExpirationDate,
                    Quantity = p.Quantity,
                    DiscountPercentage = (int)Math.Floor((decimal)p.Discount),
                    PhotoUrls = photoUrlsDict.ContainsKey(p.Id) ? photoUrlsDict[p.Id] : new List<string>()
                }).ToList();

                return Result<List<ProductDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result<List<ProductDto>>.Failure($"Failed to retrieve top deals: {ex.Message}");
            }
        }
    }
}
