using AgrifoodManagement.Util.Validations;
using AgrifoodManagement.Util.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AgrifoodManagement.Web.Models
{
    public class UpsertProductViewModel
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

        [Required]
        public int? Category { get; set; }

        public string? CategoryName { get; set; }

        public int? CartQuantity { get; set; }

        public int? WishlistQuantity { get; set; }

        public decimal EstimatedMarketPrice { get; set; }

        public bool IsPromoted { get; set; }

        public AnnouncementStatus AnnouncementStatus { get; set; }

        public List<string> PhotoUrls { get; set; } = new List<string>();
    }
}
