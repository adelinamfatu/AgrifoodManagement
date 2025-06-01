using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class MonthlyCategorySalesDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string CategoryName { get; set; }
        public double TotalSales { get; set; }
    }
}
