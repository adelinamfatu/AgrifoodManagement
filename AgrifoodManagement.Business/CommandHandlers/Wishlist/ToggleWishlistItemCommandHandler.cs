using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Wishlist
{
    public class ToggleWishlistItemCommandHandler : IRequestHandler<ToggleWishlistItemCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public ToggleWishlistItemCommandHandler(ApplicationDbContext context) 
            => _context = context;

        public async Task<bool> Handle(ToggleWishlistItemCommand cmd, CancellationToken ct)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == cmd.Email, ct);
            if (user == null)
                throw new Exception($"No user with email {cmd.Email}");

            var existingWishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.ProductId == cmd.ProductId, ct);

            if (existingWishlistItem != null)
            {
                _context.WishlistItems.Remove(existingWishlistItem);
                await _context.SaveChangesAsync(ct);
                return false;
            }
            else
            {
                _context.WishlistItems.Add(new WishlistItem
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    ProductId = cmd.ProductId
                });
                await _context.SaveChangesAsync(ct);
                return true;
            }
        }
    }
}
