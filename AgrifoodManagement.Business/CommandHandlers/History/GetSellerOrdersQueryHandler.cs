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
    public class GetSellerOrdersQueryHandler : IRequestHandler<GetSellerOrdersQuery, List<OrderTreeDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetSellerOrdersQueryHandler(ApplicationDbContext context)
            => _context = context;

        public async Task<List<OrderTreeDto>> Handle(GetSellerOrdersQuery request, CancellationToken ct)
        {
            var details = await _context.OrderDetails
                .Where(d => d.Seller!.Email == request.SellerEmail)
                .Include(d => d.Order)
                .ThenInclude(o => o!.Buyer)
                .Include(d => d.Product)
                .ToListAsync(ct);

            // Group them by Order to build tree nodes
            var byOrder = details.GroupBy(d => d.Order!);

            var result = byOrder.Select(g => {
                var order = g.Key;
                return new OrderTreeDto
                {
                    Id = order.Id,
                    Name = string.Join(", ", g.Select(d => d.Product!.Name)),
                    BuyerPhone = order.Buyer?.PhoneNumber ?? "(no phone)",
                    Status = order.Status.ToString(),
                    Quantity = string.Join(" + ",
                                  g.GroupBy(d => d.Product!.UnitOfMeasurement)
                                   .Select(gr => $"{gr.Sum(d => d.Quantity)} {gr.Key}")),
                    Total = g.Sum(d => d.Quantity * d.UnitPrice),
                    Children = g.Select(d => new OrderTreeDto
                    {
                        Id = d.Id,
                        Name = d.Product!.Name,
                        Quantity = $"{d.Quantity} {d.Product.UnitOfMeasurement}",
                        Total = d.Quantity * d.UnitPrice
                    })
                    .ToList()
                };
            })
            .ToList();

            return result;
        }
    }
}
