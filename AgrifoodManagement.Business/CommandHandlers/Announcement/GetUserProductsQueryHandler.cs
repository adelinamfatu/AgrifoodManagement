using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class GetUserProductsQueryHandler : IRequestHandler<GetUserProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetUserProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CurrentPrice = p.CurrentPrice ?? p.OriginalPrice,
                    Quantity = p.Quantity,
                    UnitOfMeasurement = p.UnitOfMeasurement,
                    ExpirationDate = p.ExpirationDate,
                    CategoryId = p.ProductCategoryId,
                    CategoryName = p.ProductCategory != null ? p.ProductCategory.Name : "Uncategorized",
                    ViewCount = 15,
                    InquiryCount = 20,
                    EstimatedMarketPrice = 50,
                    IsPromoted = p.IsPromoted,
                    AnnouncementStatus = p.AnnouncementStatus,
                    PhotoUrls = _context.ExtendedProperties
                        .Where(ep => ep.EntityId == p.Id && ep.Key == "PhotoUrl")
                        .Select(ep => ep.Value)
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
