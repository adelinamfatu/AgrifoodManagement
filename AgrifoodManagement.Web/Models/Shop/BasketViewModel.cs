namespace AgrifoodManagement.Web.Models.Shop
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
    }

    public class BasketItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public decimal PricePerUnit { get; set; }
        public int QuantityOrdered { get; set; }
        public int MaxQuantity { get; set; }
        public string Category { get; set; } = "";
    }
}
