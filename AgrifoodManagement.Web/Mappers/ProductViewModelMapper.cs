using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models.Shop;

namespace AgrifoodManagement.Web.Mappers
{
    public static class ProductViewModelMapper
    {
        public static List<ProductViewModel> Map(List<ProductDto> dtos)
        {
            return dtos.Select(MapOne).ToList();
        }

        public static ProductViewModel MapOne(ProductDto dto)
        {
            return new ProductViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CategoryName = dto.CategoryName, 
                ImageUrls = dto.PhotoUrls,
                CurrentPrice = dto.CurrentPrice,
                OriginalPrice = dto.OriginalPrice,
                Quantity = dto.Quantity,
                UnitOfMeasurement = dto.UnitOfMeasurement,
                ExpiryDate = dto.ExpirationDate,
                Rating = 4.5m,
                ReviewCount = 500,
                IsFavorited = dto.IsFavorited,
                Badge = dto.DiscountPercentage > 0 ? $"{dto.DiscountPercentage}% OFF" : string.Empty
            };
        }
    }
}
