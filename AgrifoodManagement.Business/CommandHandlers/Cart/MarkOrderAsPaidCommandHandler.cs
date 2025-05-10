using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public MarkOrderAsPaidCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            order.Status = OrderStatus.Processing;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
