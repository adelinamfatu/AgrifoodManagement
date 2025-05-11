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
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateOrderStatusCommandHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(UpdateOrderStatusCommand req, CancellationToken ct)
        {
            var order = await _context.Orders.FindAsync(new object[] { req.OrderId }, ct);
            if (order == null) return false;

            if (!IsValidTransition(order.Status, req.NewStatus)) return false;

            order.Status = req.NewStatus;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        private bool IsValidTransition(OrderStatus oldS, OrderStatus newS)
        {
            return (oldS, newS) switch
            {
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Processing, OrderStatus.Canceled) => true,
                (OrderStatus.Shipped, OrderStatus.Completed) => true,
                _ => false
            };
        }
    }
}
