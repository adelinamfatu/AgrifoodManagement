namespace AgrifoodManagement.Web.Models.Shop
{
    public class ProductViewModel
    {
        public string Badge { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}
