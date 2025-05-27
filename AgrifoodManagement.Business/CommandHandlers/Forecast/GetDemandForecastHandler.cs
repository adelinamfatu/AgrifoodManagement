using AgrifoodManagement.Business.Queries.Forecast;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Forecast
{
    public class GetDemandForecastHandler : IRequestHandler<GetDemandForecastQuery, DemandForecastDto>
    {
        private readonly IMediator _mediator;
        private readonly IForecastService _forecastService;

        public GetDemandForecastHandler(IMediator mediator,
                                        IForecastService forecastService)
        {
            _mediator = mediator;
            _forecastService = forecastService;
        }

        public async Task<DemandForecastDto> Handle(GetDemandForecastQuery req, CancellationToken ct)
        {
            var history = await _mediator.Send(new GetProductSalesHistoryQuery(
                Guid.Parse(req.ProductId), req.From, req.To, req.Granularity), ct);

            var forecastResult = await _forecastService.PredictAsync(
                Guid.Parse(req.ProductId), req.From, req.To, req.Granularity);

            return new DemandForecastDto
            {
                History = history,
                ForecastPoints = forecastResult.ForecastPoints,
                Metrics = forecastResult.Metrics
            };
        }
    }
}
