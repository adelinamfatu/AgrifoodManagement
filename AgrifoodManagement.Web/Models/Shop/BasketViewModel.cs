namespace AgrifoodManagement.Web.Models.Shop
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
    }

    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReferenceNumber { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
    }
}
