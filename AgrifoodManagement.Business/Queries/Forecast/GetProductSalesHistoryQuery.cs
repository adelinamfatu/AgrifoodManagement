using AgrifoodManagement.Util.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Queries.Forecast
{
    public record GetProductSalesHistoryQuery(
        Guid ProductId,
        DateTime From,
        DateTime To,
        string Granularity
    ) : IRequest<List<TimeSeriesPoint>>;
}
