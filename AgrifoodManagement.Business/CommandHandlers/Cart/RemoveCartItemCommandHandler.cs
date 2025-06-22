using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public RemoveCartItemCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveCartItemCommand req, CancellationToken ct)
        {
            var detail = await _context.OrderDetails
                .Include(d => d.Order)
                .SingleOrDefaultAsync(d =>
                    d.Id == req.OrderDetailId &&
                    d.Order.Status == OrderStatus.InCart, ct);

            if (detail == null)
                throw new KeyNotFoundException("An error occurred while removing the item.");

            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
