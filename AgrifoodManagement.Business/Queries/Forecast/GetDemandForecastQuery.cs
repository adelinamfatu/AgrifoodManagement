using AgrifoodManagement.Util.Models;
using MediatR;

namespace AgrifoodManagement.Business.Queries.Forecast
{
    public record GetDemandForecastQuery(
        string ProductId,
        DateTime From,
        DateTime To,
        string Granularity
    ) : IRequest<DemandForecastDto>;
}
