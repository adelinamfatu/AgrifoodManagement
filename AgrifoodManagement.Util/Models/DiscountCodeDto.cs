using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class DiscountCodeDto
    {
        public string Code { get; set; }
        public DiscountType Type { get; set; }
        public decimal Value { get; set; }
    }
}
