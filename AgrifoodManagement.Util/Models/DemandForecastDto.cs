using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class DemandForecastDto
    {
        public List<TimeSeriesPoint> History { get; set; }
        public List<ForecastPoint> ForecastPoints { get; set; }
        public ForecastMetrics Metrics { get; set; }
    }
}
