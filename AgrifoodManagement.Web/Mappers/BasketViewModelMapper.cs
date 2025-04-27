using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models.Shop;

namespace AgrifoodManagement.Web.Mappers
{
    public static class BasketViewModelMapper
    {
        public static BasketViewModel Map(CartDto dto)
        {
            var items = dto.Items
                .Select(MapItem)
                .ToList();

            return new BasketViewModel
            {
                Items = items,
                SubTotal = dto.SubTotal,
                ShippingCost = dto.ShippingCost,
                Total = dto.Total
            };
        }

        private static BasketItemViewModel MapItem(CartItemDto i)
        {
            return new BasketItemViewModel
            {
                Id = i.Id,
                Name = i.Name,
                ImageUrl = i.ImageUrl,
                PricePerUnit = i.UnitPrice,
                QuantityOrdered = i.QuantityOrdered,
                Price = i.QuantityOrdered * i.UnitPrice,
                Category = i.CategoryName
            };
        }
    }
}
