using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class CreateCheckoutSessionDto
    {
        public Guid OrderId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string DeliveryMethod { get; set; }
        public string DiscountCode { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
