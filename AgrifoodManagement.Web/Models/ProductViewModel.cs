using AgrifoodManagement.Util.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AgrifoodManagement.Web.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 999999)]
        public double Quantity { get; set; }

        public MeasurementUnit? UnitOfMeasurement { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string? Location { get; set; }

        public int? Category { get; set; }

        public int ViewCount { get; set; }

        public int InquiryCount { get; set; }

        public string? DemandForecast { get; set; }

        public decimal EstimatedMarketPrice { get; set; }

        public bool IsArchived { get; set; }

        public bool IsPromoted { get; set; }

        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
