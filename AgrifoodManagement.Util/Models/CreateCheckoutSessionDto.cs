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
        public string DeliveryMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
