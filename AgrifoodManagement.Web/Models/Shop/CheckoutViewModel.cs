using System.ComponentModel.DataAnnotations;

namespace AgrifoodManagement.Web.Models.Shop
{
    public class CheckoutViewModel
    {
        // Contact Information
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; } = "+46";

        // Delivery Information
        [Required(ErrorMessage = "Postal code is required")]
        public string PostalCode { get; set; }

        public string DeliveryMethod { get; set; } = "SameDay";

        public string PaymentMethod { get; set; } = "IPay";

        public int ItemCount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmount { get; set; }

        // Selected Card Information (for display)
        public string CardNumber { get; set; }
        public string CardExpiryDate { get; set; }
        public string CardholderName { get; set; }
    }
}
