namespace AgrifoodManagement.Web.Models.Wishlist
{
    public class WishlistItemViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MeasurementUnit { get; set; }
        public string ImageUrl { get; set; }
    }

    public class WishlistViewModel
    {
        public List<WishlistItemViewModel> Items { get; set; }
    }
}
