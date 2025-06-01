using AgrifoodManagement.Util.Models;
using MediatR;

namespace AgrifoodManagement.Business.Queries.Report
{
    public record GetMonthlySalesQuery(int MonthsBack = 6) : IRequest<List<MonthlyCategorySalesDto>>;
}
