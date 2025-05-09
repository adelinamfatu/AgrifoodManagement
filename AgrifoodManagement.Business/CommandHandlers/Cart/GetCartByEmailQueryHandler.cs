using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class GetCartByEmailQueryHandler : IRequestHandler<GetCartByEmailQuery, CartDto>
    {
        private readonly ApplicationDbContext _context;

        public GetCartByEmailQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto> Handle(GetCartByEmailQuery request, CancellationToken ct)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails!)
                    .ThenInclude(d => d.Product!)
                        .ThenInclude(p => p.ProductCategory)
                .Include(o => o.Buyer)
                .SingleOrDefaultAsync(o =>
                    o.Buyer!.Email == request.BuyerEmail &&
                    o.Status == OrderStatus.InCart ||
                    o.Status == OrderStatus.Pending,
                    ct);

            if (order == null)
                return new CartDto();

            var productIds = order.OrderDetails!
                .Select(d => d.ProductId)
                .Distinct()
                .ToList();

            var photoUrlDict = await _context.ExtendedProperties
                .Where(ep => ep.Key == "PhotoUrl" && productIds.Contains(ep.EntityId))
                .GroupBy(ep => ep.EntityId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Select(x => x.Value).FirstOrDefault() ?? "",
                    ct);

            var items = order.OrderDetails!
                .Select(d => new CartItemDto
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    Name = d.Product!.Name,
                    CategoryName = d.Product.ProductCategory!.Name,
                    UnitPrice = d.UnitPrice,
                    QuantityOrdered = d.Quantity,
                    MaxQuantity = d.Product.Quantity,
                    ImageUrl = photoUrlDict.TryGetValue(d.ProductId, out var url)
                        ? url
                        : "/images/no-image-placeholder.png"
                })
                .ToList();

            return new CartDto
            {
                OrderId = order.Id,
                Items = items,
                ShippingCost = 5m
            };
        }
    }
}
