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

        private static ProductViewModel MapOne(ProductDto dto)
        {
            return new ProductViewModel
            {
                Name = dto.Name,
                ImageUrl = dto.PhotoUrls.FirstOrDefault(),
                CurrentPrice = dto.CurrentPrice,
                OriginalPrice = dto.OriginalPrice,
                AverageRating = dto.AverageRating,
                RatingCount = dto.RatingCount,
            };
        }
    }
}
