using AgrifoodManagement.Business.Queries.Report;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Report
{
    public class ReportQueryHandlers :
        IRequestHandler<GetOrderStatusDistributionQuery, List<OrderStatusShareDto>>,
        IRequestHandler<GetMonthlySalesQuery, List<MonthlyCategorySalesDto>>,
        IRequestHandler<GetQuarterlySalesQuery, List<QuarterlyProductSalesDto>>
    {
        private readonly ApplicationDbContext _context;

        public ReportQueryHandlers(ApplicationDbContext context) => _context = context;

        public async Task<List<OrderStatusShareDto>> Handle(GetOrderStatusDistributionQuery req, CancellationToken ct)
        {
            var byStatus = await _context.Orders
                .Where(o => o.Status != OrderStatus.Pending)
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusShareDto
                {
                    Status = g.Key == OrderStatus.InCart
                        ? "In Cart"
                        : g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync(ct);

            return byStatus;
        }

        public async Task<List<MonthlyCategorySalesDto>> Handle(GetMonthlySalesQuery req, CancellationToken ct)
        {
            var cutoff = DateTime.UtcNow.AddMonths(-req.MonthsBack);

            var raw = await _context.OrderDetails
                .Where(od => od.Order!.Status == OrderStatus.Completed
                          && od.Order.OrderedAt >= cutoff
                          && od.Product!.ProductCategory != null)
                .Select(od => new
                {
                    Year = od.Order.OrderedAt.Value.Year,
                    Month = od.Order.OrderedAt.Value.Month,
                    CategoryName = od.Product.ProductCategory.Name!,
                    Amount = od.UnitPrice * od.Quantity
                })
                .ToListAsync(ct);

            var grouped = raw
                .GroupBy(x => new { x.Year, x.Month, x.CategoryName })
                .Select(g => new MonthlyCategorySalesDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    CategoryName = g.Key.CategoryName,
                    TotalSales = (double)g.Sum(x => x.Amount)
                })
                .ToList();

            var periods = Enumerable
                .Range(0, req.MonthsBack + 1)
                .Select(i => DateTime.UtcNow.AddMonths(-i))
                .OrderBy(d => d.Year).ThenBy(d => d.Month)
                .ToList();

            var result = new List<MonthlyCategorySalesDto>();

            foreach (var d in periods)
            {
                var year = d.Year;
                var month = d.Month;

                var categoriesInGroup = grouped
                    .Where(x => x.Year == year && x.Month == month)
                    .Select(x => x.CategoryName)
                    .Distinct()
                    .ToList();

                foreach (var cat in categoriesInGroup)
                {
                    var found = grouped
                        .FirstOrDefault(x => x.Year == year && x.Month == month && x.CategoryName == cat);

                    result.Add(new MonthlyCategorySalesDto
                    {
                        Year = year,
                        Month = month,
                        CategoryName = cat,
                        TotalSales = found?.TotalSales ?? 0
                    });
                }
            }

            return result;
        }

        public async Task<List<QuarterlyProductSalesDto>> Handle(GetQuarterlySalesQuery req, CancellationToken ct)
        {
            var raw = await _context.OrderDetails
                .Where(od => od.Order!.Status == OrderStatus.Completed
                          && od.Order.OrderedAt.HasValue)
                .Select(od => new
                {
                    OrderedAt = od.Order.OrderedAt!.Value,
                    ProductName = od.Product!.Name,
                    Amount = od.UnitPrice * od.Quantity
                })
                .ToListAsync(ct);

            var grouped = raw
                .GroupBy(x => new
                {
                    Quarter = ((x.OrderedAt.Month - 1) / 3) + 1, // 1..4
                    x.ProductName
                })
                .Select(g => new
                {
                    Quarter = g.Key.Quarter,           // 1, 2, 3 or 4
                    ProductName = g.Key.ProductName,
                    TotalSales = (double)g.Sum(x => x.Amount)
                })
                .ToList();

            var result = new List<QuarterlyProductSalesDto>();

            var allProducts = grouped
                .Select(x => x.ProductName)
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            for (int q = 1; q <= 4; q++)
            {
                string quarterLabel = $"Q{q}";
                foreach (var productName in allProducts)
                {
                    var found = grouped
                        .FirstOrDefault(x => x.Quarter == q && x.ProductName == productName);

                    result.Add(new QuarterlyProductSalesDto
                    {
                        Period = quarterLabel,
                        ProductName = productName,
                        Sales = found?.TotalSales ?? 0
                    });
                }
            }

            return result;
        }
    }
}
