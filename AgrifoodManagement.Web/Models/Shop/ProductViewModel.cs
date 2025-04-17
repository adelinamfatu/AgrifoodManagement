using AgrifoodManagement.Util.ValueObjects;

namespace AgrifoodManagement.Web.Models.Shop
{
    public class ProductViewModel
    {
        public string Badge { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public double Quantity { get; set; }
        public MeasurementUnit? UnitOfMeasurement { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpiringSoon => ExpiryDate.Subtract(DateTime.Now).TotalDays <= 3;
    }
}
