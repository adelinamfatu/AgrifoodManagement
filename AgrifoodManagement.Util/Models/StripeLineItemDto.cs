using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class StripeLineItemDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long UnitAmount { get; set; }
        public string Currency { get; set; } = "ron";
        public long Quantity { get; set; } = 1;
    }
}
