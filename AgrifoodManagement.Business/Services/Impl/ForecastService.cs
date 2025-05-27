using AgrifoodManagement.Business.Queries.Forecast;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace AgrifoodManagement.Business.Services.Impl
{
    public class ForecastService : IForecastService
    {
        private readonly IMediator _mediator;
        private readonly MLContext _ml;

        public ForecastService(IMediator mediator)
        {
            _mediator = mediator;
            _ml = new MLContext(seed: 0);
        }

        public async Task<ForecastResult> PredictAsync(Guid productId, DateTime from, DateTime to, string granularity)
        {
            // Get the real history
            var history = await _mediator.Send(
                new GetProductSalesHistoryQuery(productId, from, to, granularity));

            if (history == null || history.Count < 2)
            {
                return new ForecastResult
                {
                    Metrics = new ForecastMetrics { Mape = 0, Rmse = 0 },
                    ForecastPoints = new List<ForecastPoint>()
                };
            }

            // Fit a simple linear regression (y = a + b·x) on the history indices
            int n = history.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumXX = 0;
            for (int i = 0; i < n; i++)
            {
                double x = i;
                double y = history[i].Value;
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumXX += x * x;
            }
            double slope = (n * sumXY - sumX * sumY)
                           / (n * sumXX - sumX * sumX);
            double intercept = (sumY - slope * sumX) / n;

            // Generate forecasts for the next horizon
            int horizon = granularity.ToLower() switch
            {
                "month" => 6,
                "week" => 4,
                _ => 7
            };
            Func<int, DateTime> advance = granularity.ToLower() switch
            {
                "month" => i => to.AddMonths(i),
                "week" => i => to.AddDays(7 * i),
                _ => i => to.AddDays(i)
            };

            var forecastPoints = new List<ForecastPoint>();
            var errors = new List<double>();
            var sqErrors = new List<double>();

            // Compute in-sample error if desired
            for (int i = 0; i < n; i++)
            {
                double predIn = intercept + slope * i;
                double actual = history[i].Value;
                double err = actual - predIn;
                if (actual != 0)
                {
                    errors.Add(Math.Abs(err / actual));
                    sqErrors.Add(err * err);
                }
            }
            double mape = errors.Any() ? errors.Average() : 0;
            double rmse = sqErrors.Any() ? Math.Sqrt(sqErrors.Average()) : 0;

            // Build future points
            foreach (var i in Enumerable.Range(1, horizon))
            {
                double pred = intercept + slope * (n - 1 + i);
                forecastPoints.Add(new ForecastPoint
                {
                    Date = advance(i),
                    Predicted = pred,
                    Lower = pred * 0.90,
                    Upper = pred * 1.10,
                });
            }

            // Return the result
            return new ForecastResult
            {
                Metrics = new ForecastMetrics
                {
                    Mape = mape,
                    Rmse = rmse
                },
                ForecastPoints = forecastPoints
            };
        }

        private class SalesData
        {
            public float Value { get; set; }
        }

        private class ForecastOutput
        {
            public float Value { get; set; }

            public float[] ForecastedValues { get; set; }
            public float[] LowerBounds { get; set; }
            public float[] UpperBounds { get; set; }
        }
    }
}
