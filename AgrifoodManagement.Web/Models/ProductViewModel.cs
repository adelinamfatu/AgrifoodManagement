namespace AgrifoodManagement.Web.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public string? UnitOfMeasurement { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Additional properties for farmer perspective
        public string Location { get; set; }
        public string? Category { get; set; }
        public int ViewCount { get; set; }
        public int InquiryCount { get; set; }
        public string DemandForecast { get; set; } // High, Medium, Low
        public decimal EstimatedMarketPrice { get; set; }
        public bool IsArchived { get; set; }

        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
