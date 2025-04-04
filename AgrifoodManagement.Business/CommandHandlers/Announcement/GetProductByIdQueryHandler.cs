using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgrifoodManagement.Util.Models;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly ApplicationDbContext _context;

        public GetProductByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    UnitOfMeasurement = p.UnitOfMeasurement,
                    ExpirationDate = p.ExpirationDate,
                    CategoryId = p.ProductCategoryId,
                    CategoryName = p.ProductCategory.Name,
                    IsPromoted = p.IsPromoted,
                    AnnouncementStatus = p.AnnouncementStatus,
                    PhotoUrls = _context.ExtendedProperties
                        .Where(ep => ep.EntityId == p.Id && ep.Key == "PhotoUrl")
                        .Select(ep => ep.Value)
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return product;
        }
    }
}
