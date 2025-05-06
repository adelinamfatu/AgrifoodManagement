using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class DiscountRequest
    {
        public string Code { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
    }
}
