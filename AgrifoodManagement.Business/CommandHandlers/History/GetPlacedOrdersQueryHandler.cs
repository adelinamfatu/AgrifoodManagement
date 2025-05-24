using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.History
{
    public class GetPlacedOrdersQueryHandler : IRequestHandler<GetProcessedOrdersQuery, List<OrderTreeDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetPlacedOrdersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderTreeDto>> Handle(GetProcessedOrdersQuery request, CancellationToken ct)
        {
            var orders = await _context.Orders
                .Where(o => o.Buyer!.Email == request.BuyerEmail &&
                           (o.Status == OrderStatus.Processing ||
                            o.Status == OrderStatus.Shipped ||
                            o.Status == OrderStatus.Canceled ||
                            o.Status == OrderStatus.Completed))
                .Include(o => o.OrderDetails!)
                .ThenInclude(d => d.Product!)
                .ThenInclude(p => p.Seller!)
                .ToListAsync(ct);

            var result = orders.Select(order => new OrderTreeDto
            {
                Id = order.Id,
                Name = string.Join(", ", order.OrderDetails!.Select(d => d.Product!.Name)),
                Delivery = order.DeliveryMethod,
                Status = order.Status.ToString(),
                Quantity = string.Join(" + ",
                    order.OrderDetails!
                        .GroupBy(d => d.Product!.UnitOfMeasurement)
                        .Select(g => $"{g.Sum(d => d.Quantity)} {g.Key}")),
                Total = order.OrderDetails!.Sum(d => d.Quantity * d.UnitPrice),
                Children = order.OrderDetails.Select(od => new OrderTreeDto
                {
                    Id = od.Id,
                    Name = od.Product!.Name,
                    Quantity = $"{od.Quantity} {od.Product.UnitOfMeasurement}",
                    Total = od.Quantity * od.UnitPrice,
                    SellerPhone = od.Seller.PhoneNumber,
                }).ToList()
            }).ToList();

            return result;
        }
    }
}
