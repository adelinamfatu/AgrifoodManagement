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
    public class GetProductsPerPageQueryHandler : IRequestHandler<GetProductsPerPageQuery, Result<PagedResult<ProductDto>>>
    {
        private readonly ApplicationDbContext _context;

        public GetProductsPerPageQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedResult<ProductDto>>> Handle(GetProductsPerPageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Products
                    .Include(p => p.ProductCategory)
                    .AsNoTracking()
                    .OrderBy(p => p.Name);

                var totalCount = await query.CountAsync(cancellationToken);

                var products = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var productIds = products.Select(p => p.Id).ToList();

                var photoUrlsDict = await _context.ExtendedProperties
                    .Where(ep => ep.Key == "PhotoUrl")
                    .GroupBy(ep => ep.EntityId)
                    .ToDictionaryAsync(g => g.Key, g => new List<string> 
                        { 
                            g.Select(x => x.Value).FirstOrDefault() 
                        }, cancellationToken);

                var items = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CurrentPrice = p.Price,
                    Quantity = p.Quantity,
                    UnitOfMeasurement = p.UnitOfMeasurement,
                    ExpirationDate = p.ExpirationDate,
                    CategoryId = p.ProductCategoryId,
                    CategoryName = p.ProductCategory?.Name ?? "Uncategorized",
                    ViewCount = 15,
                    InquiryCount = 20,
                    EstimatedMarketPrice = 50,
                    IsPromoted = p.IsPromoted,
                    AnnouncementStatus = p.AnnouncementStatus,
                    PhotoUrls = photoUrlsDict.ContainsKey(p.Id) ? photoUrlsDict[p.Id] : new List<string>()
                }).ToList();

                var pagedResult = new PagedResult<ProductDto>
                {
                    Items = items,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
                };

                return Result<PagedResult<ProductDto>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<ProductDto>>.Failure($"Error retrieving products: {ex.Message}");
            }
        }
    }
}
