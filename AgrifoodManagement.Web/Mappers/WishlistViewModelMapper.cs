using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models.Wishlist;

namespace AgrifoodManagement.Web.Mappers
{
    public static class WishlistViewModelMapper
    {
        public static WishlistViewModel Map(List<WishlistItemDto> dto)
        {
            return new WishlistViewModel
            {
                Items = dto.Select(MapItem).ToList()
            };
        }

        private static WishlistItemViewModel MapItem(WishlistItemDto i)
        {
            return new WishlistItemViewModel
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Price = i.Price,
                MeasurementUnit = i.MeasurementUnit.ToString(),
                Description = i.Description,
                ImageUrl = i.ImageUrl
            };
        }
    }
}
