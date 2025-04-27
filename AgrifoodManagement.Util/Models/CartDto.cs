using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class CartDto
    {
        public IReadOnlyList<CartItemDto> Items { get; init; } = Array.Empty<CartItemDto>();
        public decimal SubTotal => Items.Sum(i => i.UnitPrice * i.QuantityOrdered);
        public decimal ShippingCost { get; init; } = 5m;
        public decimal Total => SubTotal + ShippingCost;
    }

    public class CartItemDto
    {
        public Guid Id { get; init; }
        public Guid ProductId { get; init; }
        public string Name { get; init; } = "";
        public string CategoryName { get; init; } = "";
        public string ImageUrl { get; init; } = "";
        public decimal UnitPrice { get; init; }
        public int QuantityOrdered { get; init; }
    }
}
