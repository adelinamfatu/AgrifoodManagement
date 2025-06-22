using AgrifoodManagement.Util.ValueObjects;

namespace AgrifoodManagement.Web.Models.Shop
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Badge { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public double Quantity { get; set; }
        public string CategoryName { get; set; }
        public MeasurementUnit? UnitOfMeasurement { get; set; }
        public string UnitOfMeasurementText => UnitOfMeasurement.ToString();
        public DateTime ExpiryDate { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string? ProducerName { get; set; }
        public bool IsFavorited { get; set; }
        public bool IsExpiringSoon => ExpiryDate.Subtract(DateTime.Now).TotalDays <= 3;
    }
}
