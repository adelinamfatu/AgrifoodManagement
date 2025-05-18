using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Wishlist
{
    public class GetUserWishlistQueryHandler : IRequestHandler<GetUserWishlistQuery, List<WishlistItemDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetUserWishlistQueryHandler(ApplicationDbContext context) 
            => _context = context;

        public async Task<List<WishlistItemDto>> Handle(GetUserWishlistQuery q, CancellationToken ct)
        {
            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == q.Email, ct);

            if (user == null)
                return new List<WishlistItemDto>();

            var items = await _context.WishlistItems
                .Where(w => w.UserId == user.Id)
                .Select(w => new WishlistItemDto
                {
                    ProductId = w.ProductId,
                    Name = w.Product.Name,
                    Price = w.Product.CurrentPrice ?? w.Product.OriginalPrice,
                    MeasurementUnit = w.Product.UnitOfMeasurement,
                    ImageUrl = _context.ExtendedProperties
                        .Where(ep => ep.EntityId == w.ProductId && ep.Key == "PhotoUrl")
                        .Select(ep => ep.Value)
                        .FirstOrDefault()
                        ?? "/images/placeholder.png"
                })
                .ToListAsync(ct);

            return items;
        }
    }
}
