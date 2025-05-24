namespace AgrifoodManagement.Web.Models
{
    public class OrderHistoryViewModel
    {
        public List<OrderTreeRowViewModel> Orders { get; set; } = new();
    }

    public class OrderTreeRowViewModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = "";
        public string Delivery { get; set; } = "";
        public string BuyerPhone { get; set; } = "";
        public string SellerPhone { get; set; } = "";
        public string Status { get; set; } = "";
        public string? Quantity { get; set; }
        public decimal? Total { get; set; }
        public bool CanReview { get; set; }
        public List<OrderTreeRowViewModel>? Children { get; set; }
    }
}
