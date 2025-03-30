using AgrifoodManagement.Util.Validations;
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
        [Range(0.01, 999999.99, ErrorMessage = "Price must be a positive value greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 999999, ErrorMessage = "Quantity must be greater than 0.")]
        public double Quantity { get; set; }

        public MeasurementUnit? UnitOfMeasurement { get; set; }

        [FutureDate(ErrorMessage = "Expiration date must be in the future.")]
        public DateTime ExpirationDate { get; set; } = DateTime.Now;

        public string? Location { get; set; }

        [Required]
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
