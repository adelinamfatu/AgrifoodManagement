using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models;

namespace AgrifoodManagement.Web.Mappers
{
    public static class CategoryViewModelMapper
    {
        public static List<CategoryViewModel> Map(List<CategoryDto> dtos)
        {
            return dtos.Select(MapOne).ToList();
        }

        private static CategoryViewModel MapOne(CategoryDto dto)
        {
            return new CategoryViewModel
            {
                Id = dto.Id,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                Children = dto.Children.Select(MapOne).ToList()
            };
        }
    }
}
