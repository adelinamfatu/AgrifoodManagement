using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public ConfirmOrderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);
            
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            order.Status = Util.ValueObjects.OrderStatus.Pending;
            order.TotalAmount = request.TotalAmount;
            order.OrderedAt = DateTime.UtcNow;
            order.DeliveryMethod = request.DeliveryMethod;
            order.DeliveryFee = request.DeliveryFee ?? 0;
            order.DiscountCodeId = !string.IsNullOrWhiteSpace(request.DiscountCode) ? request.DiscountCode : null;
            order.DeliveryAddress = request.DeliveryAddress;
            order.PhoneNumber = request.PhoneNumber;

            var (lat, lon) = await GeocodeAddressAsync(request.DeliveryAddress);
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

        private async Task<(double? lat, double? lon)> GeocodeAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return (null, null);

            address = address.Replace(",", " ");
            var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "AgroFoodApp");

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return (null, null);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<NominatimResult>>(content);

            if (result?.Any() == true)
            {
                var loc = result.First();
                return (double.Parse(loc.lat), double.Parse(loc.lon));
            }

            return (null, null);
        }
    }
}
