namespace AgrifoodManagement.Web.Models.Shop
{
    public class ShopViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
