using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public UpdateCartItemQuantityCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCartItemQuantityCommand req, CancellationToken ct)
        {
            var detail = await _context.OrderDetails
                .Include(d => d.Order)
                .Include(d => d.Product)
                .SingleOrDefaultAsync(d =>
                    d.Id == req.OrderDetailId &&
                    d.Order.Status == OrderStatus.InCart, ct);

            if (detail == null)
                throw new KeyNotFoundException("An error occurred while removing the item.");

            detail.Quantity = (int)Math.Clamp(req.NewQuantity, 0, detail.Product.Quantity);
            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
