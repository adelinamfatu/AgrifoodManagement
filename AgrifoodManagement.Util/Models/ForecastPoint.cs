using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class ForecastPoint
    {
        public DateTime Date { get; set; }
        public double Predicted { get; set; }
        public double Lower { get; set; }
        public double Upper { get; set; }
        public double? Actual { get; set; }
    }
}
