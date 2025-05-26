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

namespace AgrifoodManagement.Business.CommandHandlers.Stock
{
    public class GetProductStocksQueryHandler : IRequestHandler<GetProductStocksQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductStocksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetProductStocksQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.UserEmail, cancellationToken);

            var now = DateTime.UtcNow;

            return await _context.Products
                .Where(p => p.ExpirationDate > now
                    && p.UserId == user!.Id)
                .Include(p => p.ProductCategory)
                .Include(p => p.Seller)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    UnitOfMeasurement = p.UnitOfMeasurement,
                    CurrentPrice = p.CurrentPrice ?? p.OriginalPrice,
                    CategoryName = p.ProductCategory != null
                                       ? p.ProductCategory.Name
                                       : "Uncategorized"
                })
                .ToListAsync(cancellationToken);
        }
    }
}
