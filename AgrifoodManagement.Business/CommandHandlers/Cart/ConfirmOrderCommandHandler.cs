using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, Guid>
    {
        private readonly ApplicationDbContext _context;
        private readonly IGeocodingService _geocoder;
        private readonly IOrderStatusTransitionValidator _validator;

        public ConfirmOrderCommandHandler(ApplicationDbContext context, IGeocodingService geocodingService, IOrderStatusTransitionValidator validator)
        {
            _context = context;
            _geocoder = geocodingService;
            _validator = validator;
        }

        public async Task<Guid> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);
            
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            if (!_validator.IsValidTransition(order.Status, OrderStatus.Pending))
            {
                throw new InvalidOperationException(
                   $"Cannot move order from {order.Status} to Pending.");
            }

            order.Status = OrderStatus.Pending;
            order.TotalAmount = request.TotalAmount;
            order.OrderedAt = DateTime.UtcNow;
            order.DeliveryMethod = request.DeliveryMethod;
            order.DeliveryFee = request.DeliveryFee ?? 0;
            order.DiscountCodeId = !string.IsNullOrWhiteSpace(request.DiscountCode) ? request.DiscountCode : null;
            order.DeliveryAddress = request.DeliveryAddress;
            order.PhoneNumber = request.PhoneNumber;

            var (lat, lon) = await _geocoder.GeocodeAddressAsync(request.DeliveryAddress);
            order.DeliveryLatitude = lat ?? 0;
            order.DeliveryLongitude = lon ?? 0;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save order: " + ex.Message, ex);
            }

            return order.Id;
        }
    }
}
