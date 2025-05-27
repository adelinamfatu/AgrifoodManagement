using AgrifoodManagement.Business.Queries.Forecast;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Forecast
{
    public class GetProductSalesHistoryHandler : IRequestHandler<GetProductSalesHistoryQuery, List<TimeSeriesPoint>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductSalesHistoryHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<TimeSeriesPoint>> Handle(GetProductSalesHistoryQuery req, CancellationToken ct)
        {
            var raw = await _context.OrderDetails
                .Where(od => od.ProductId == req.ProductId
                          && od.Order!.OrderedAt >= req.From
                          && od.Order.OrderedAt <= req.To)
                .Select(od => new {
                    Date = od.Order.OrderedAt!.Value.Date,
                    od.Quantity
                })
                .ToListAsync(ct);

            IEnumerable<IGrouping<DateTime, dynamic>> groups = req.Granularity.ToLower() switch
            {
                "month" => raw.GroupBy(x => new DateTime(x.Date.Year, x.Date.Month, 1)),
                "week" => raw.GroupBy(x => {
                    var dt = x.Date;
                    int diff = ((int)dt.DayOfWeek + 6) % 7;   // days since Monday
                    return dt.AddDays(-diff);
                }),
                _ => raw.GroupBy(x => x.Date)
            };

            var result = groups
                .Select(g => new TimeSeriesPoint
                {
                    Date = g.Key,
                    Value = g.Sum(x => (int)x.Quantity)
                })
                .OrderBy(pt => pt.Date)
                .ToList();

            return result;
        }
    }
}
