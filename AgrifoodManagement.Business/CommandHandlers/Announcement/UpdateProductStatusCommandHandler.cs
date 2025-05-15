using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class UpdateProductStatusCommandHandler : IRequestHandler<UpdateProductStatusCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateProductStatusCommandHandler(ApplicationDbContext context)
            => _context = context;

        public async Task<bool> Handle(UpdateProductStatusCommand req, CancellationToken ct)
        {
            var product = await _context.Products.FindAsync(new object[] { req.ProductId }, ct);
            if (product == null) return false;

            product.AnnouncementStatus = req.NewStatus;
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
