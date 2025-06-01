using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class QuarterlyProductSalesDto
    {
        public string Period { get; init; } = null!;
        public string ProductName { get; set; }
        public double Sales { get; init; }
    }
}
