using AgrifoodManagement.Util.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Interfaces
{
    public interface IForecastService
    {
        Task<ForecastResult> PredictAsync(Guid productId, DateTime from, DateTime to, string granularity);
    }
}
