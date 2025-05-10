using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.History
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public CancelOrderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);

            if (order == null || order.Status != OrderStatus.Processing)
                return false;

            order.Status = OrderStatus.Canceled;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
