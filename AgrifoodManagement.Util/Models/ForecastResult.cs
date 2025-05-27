using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class ForecastResult
    {
        public List<ForecastPoint> ForecastPoints { get; set; } = new();

        public ForecastMetrics Metrics { get; set; } = new();
    }
}
