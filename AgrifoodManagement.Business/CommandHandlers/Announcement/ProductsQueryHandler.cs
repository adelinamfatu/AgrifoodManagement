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
    public class ProductsQueryHandler : IRequestHandler<ProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public ProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(ProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    UnitOfMeasurement = p.UnitOfMeasurement.ToString(),
                    ExpirationDate = p.ExpirationDate,
                    Location = "Bucuresti",
                    CategoryId = p.ProductCategoryId,
                    CategoryName = p.ProductCategory != null ? p.ProductCategory.Name : "Uncategorized",
                    ViewCount = 15,
                    InquiryCount = 20,
                    DemandForecast = "High",
                    EstimatedMarketPrice = 50,
                    IsArchived = false
                })
                .ToListAsync(cancellationToken);
        }
    }
}
