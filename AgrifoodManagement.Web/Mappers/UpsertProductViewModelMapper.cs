using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models;

namespace AgrifoodManagement.Web.Mappers
{
    public static class UpsertProductViewModelMapper
    {
        public static List<UpsertProductViewModel> MapList(IEnumerable<ProductDto> dtos)
            => dtos.Select(MapForList).ToList();

        public static UpsertProductViewModel MapForList(ProductDto dto)
            => new UpsertProductViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.CurrentPrice,
                Quantity = dto.Quantity,
                UnitOfMeasurement = dto.UnitOfMeasurement,
                ExpirationDate = dto.ExpirationDate,
                Category = dto.CategoryId,
                CategoryName = dto.CategoryName,
                CartQuantity = dto.CartQuantity,
                WishlistQuantity = dto.WishlistQuantity,
                EstimatedMarketPrice = dto.EstimatedMarketPrice,
                IsPromoted = dto.IsPromoted,
                AnnouncementStatus = dto.AnnouncementStatus,
                PhotoUrls = dto.PhotoUrls
            };

        public static UpsertProductViewModel MapForDetail(ProductDto dto)
            => new UpsertProductViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.OriginalPrice,
                Quantity = dto.Quantity,
                UnitOfMeasurement = dto.UnitOfMeasurement,
                ExpirationDate = dto.ExpirationDate,
                Category = dto.CategoryId,
                CategoryName = dto.CategoryName,
                PhotoUrls = dto.PhotoUrls
            };
    }
}
