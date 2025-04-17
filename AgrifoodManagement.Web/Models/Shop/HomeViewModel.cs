namespace AgrifoodManagement.Web.Models.Shop
{
    public class HomeViewModel
    {
        public List<ProductViewModel> Deals { get; set; } = new List<ProductViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
