using AgrifoodManagement.Business.Queries.Report;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Report
{
    public class ReportQueryHandlers :
        IRequestHandler<GetCategoryShareQuery, List<CategoryShareDto>>,
        IRequestHandler<GetMonthlySalesQuery, List<MonthlySalesDto>>,
        IRequestHandler<GetQuarterlySalesQuery, List<QuarterlySalesDto>>
    {
        private readonly ApplicationDbContext _context;

        public ReportQueryHandlers(ApplicationDbContext context) => _context = context;

        public async Task<List<CategoryShareDto>> Handle(GetCategoryShareQuery req, CancellationToken ct)
        {
            var byCat = await _context.OrderDetails
                .Where(od => od.Order!.Status == OrderStatus.Completed)
                .Include(od => od.Product)
                .ThenInclude(p => p.ProductCategory)
                .GroupBy(od => od.Product!.ProductCategory!.Name!)
                .Select(g => new {
                    Category = g.Key,
                    TotalQty = g.Sum(od => od.Quantity)
                })
                .ToListAsync(ct);

            var grand = byCat.Sum(x => x.TotalQty);
            return byCat
                .Select(x => new CategoryShareDto
                {
                    Category = x.Category,
                    Percentage = grand > 0
                       ? Math.Round((double)x.TotalQty / grand * 100, 1)
                       : 0
                })
                .ToList();
        }

        public async Task<List<MonthlySalesDto>> Handle(GetMonthlySalesQuery req, CancellationToken ct)
        {
            var cutoff = DateTime.UtcNow.AddMonths(-req.MonthsBack);
            var raw = await _context.OrderDetails
                .Where(od => od.Order!.Status == OrderStatus.Completed
                          && od.Order.OrderedAt >= cutoff)
                .Select(od => new {
                    od.Order!.OrderedAt,
                    Amount = od.UnitPrice * od.Quantity
                })
                .ToListAsync(ct);

            var grouped = raw
              .GroupBy(x => new {
                  Year = x.OrderedAt!.Value.Year,
                  Month = x.OrderedAt.Value.Month
              })
              .Select(g => new {
                  g.Key.Year,
                  g.Key.Month,
                  Total = (double)g.Sum(x => x.Amount)
              })
              .ToList();

            // Build the last N+1 months in chronological order
            var periods = Enumerable
              .Range(0, req.MonthsBack + 1)
              .Select(i => DateTime.UtcNow.AddMonths(-i))
              .OrderBy(d => d)
              .ToList();

            return periods.Select(d => new MonthlySalesDto
            {
                Period = d.ToString("MMM"),
                Sales = grouped
                         .FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)
                         ?.Total ?? 0
            }).ToList();
        }

        public async Task<List<QuarterlySalesDto>> Handle(GetQuarterlySalesQuery req, CancellationToken ct)
        {
            // Sum up all completed order-details by quarter
            var raw = await _context.OrderDetails
                .Where(od => od.Order!.Status == OrderStatus.Completed
                          && od.Order.OrderedAt.HasValue)
                .Select(od => new {
                    od.Order!.OrderedAt,
                    Amount = od.UnitPrice * od.Quantity
                })
                .ToListAsync(ct);

            var grouped = raw
              .GroupBy(x => ((x.OrderedAt!.Value.Month - 1) / 3) + 1)
              .Select(g => new {
                  Quarter = g.Key,
                  Total = (double)g.Sum(x => x.Amount)
              })
              .ToList();

            return Enumerable.Range(1, 4)
              .Select(q => new QuarterlySalesDto
              {
                  Period = $"Q{q}",
                  Sales = grouped.FirstOrDefault(x => x.Quarter == q)?.Total ?? 0
              })
              .ToList();
        }
    }
}
