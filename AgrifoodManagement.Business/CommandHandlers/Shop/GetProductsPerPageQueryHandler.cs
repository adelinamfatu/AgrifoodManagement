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
                var now = DateTime.UtcNow.Date;

                var query = _context.Products
                    .Where(p => p.AnnouncementStatus == AnnouncementStatus.Published
                        && p.ExpirationDate.Date >= now)
                    .Include(p => p.ProductCategory)
                    .AsNoTracking();

                // Category filter
                if (request.CategoryId.HasValue)
                    query = query.Where(p => p.ProductCategoryId == request.CategoryId.Value);

                // Unit filter
                if (request.UnitFilter.HasValue)
                {
                    query = query.Where(p => p.UnitOfMeasurement == request.UnitFilter.Value);
                }

                // Price‐range filter
                switch (request.PriceRange)
                {
                    case "under-15":
                        query = query.Where(p => p.CurrentPrice < 15);
                        break;
                    case "15-25":
                        query = query.Where(p => p.CurrentPrice >= 15 && p.CurrentPrice <= 25);
                        break;
                    case "25-45":
                        query = query.Where(p => p.CurrentPrice >= 25 && p.CurrentPrice <= 45);
                        break;
                    case "over-45":
                        query = query.Where(p => p.CurrentPrice > 45);
                        break;
                }

                // Sorting
                query = request.SortBy switch
                {
                    "price-low" => query.OrderBy(p => p.CurrentPrice),
                    "price-high" => query.OrderByDescending(p => p.CurrentPrice),
                    "newest" => query.OrderByDescending(p => p.TimePosted),
                    _ => query.OrderBy(p => p.ExpirationDate),
                };

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

                List<Guid> favIds = new();
                if (!string.IsNullOrEmpty(request.UserEmail))
                {
                    var user = await _context.Users
                                 .AsNoTracking()
                                 .SingleOrDefaultAsync(u => u.Email == request.UserEmail,
                                                        cancellationToken);
                    if (user != null)
                    {
                        favIds = await _context.WishlistItems
                            .AsNoTracking()
                            .Where(w => w.UserId == user.Id
                                     && productIds.Contains(w.ProductId))
                            .Select(w => w.ProductId)
                            .ToListAsync(cancellationToken);
                    }
                }

                var items = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    OriginalPrice = p.OriginalPrice,
                    CurrentPrice = p.CurrentPrice ?? p.OriginalPrice,
                    Quantity = p.Quantity,
                    UnitOfMeasurement = p.UnitOfMeasurement,
                    ExpirationDate = p.ExpirationDate,
                    CategoryId = p.ProductCategoryId,
                    IsPromoted = p.IsPromoted,
                    IsFavorited = favIds.Contains(p.Id),
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
