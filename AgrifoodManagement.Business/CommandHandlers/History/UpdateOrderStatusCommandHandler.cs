using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Services.Interfaces;
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
        private readonly IOrderStatusTransitionValidator _validator;

        public UpdateOrderStatusCommandHandler(ApplicationDbContext context, IOrderStatusTransitionValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand req, CancellationToken ct)
        {
            var order = await _context.Orders.FindAsync(new object[] { req.OrderId }, ct);
            if (order == null) return false;

            if (!_validator.IsValidTransition(order.Status, req.NewStatus))
                return false;

            order.Status = req.NewStatus;
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
