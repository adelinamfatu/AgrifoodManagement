using AgrifoodManagement.Util.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgrifoodManagement.Web.Models.Forecast
{
    public class ForecastViewModel
    {
        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();
        public string SelectedProductId { get; set; }

        public DateTime DefaultFrom { get; set; }
        public DateTime DefaultTo { get; set; }

        public string DefaultGranularity { get; set; }
        public List<SelectListItem> GranularityOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem("Daily", "day"),
            new SelectListItem("Weekly", "week"),
            new SelectListItem("Monthly", "month"),
        };

        public IEnumerable<TimeSeriesPoint> History { get; set; } = new List<TimeSeriesPoint>();

        public IEnumerable<ForecastPoint> Forecast { get; set; } = new List<ForecastPoint>();

        public ForecastMetrics Metrics { get; set; } = new ForecastMetrics();
    }
}
