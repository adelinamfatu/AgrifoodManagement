using System.ComponentModel.DataAnnotations;

namespace AgrifoodManagement.Web.Models.Shop
{
    public class CheckoutViewModel
    {
        public Guid OrderId { get; set; }

        // Contact Information
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; } = "+46";

        // Delivery Information
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DeliveryMethod { get; set; } = "Normal";
        public string PaymentMethod { get; set; } = "IPay";

        public int ItemCount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public string DiscountCode { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmount => Subtotal + DeliveryFee - Discount;
    }
}
