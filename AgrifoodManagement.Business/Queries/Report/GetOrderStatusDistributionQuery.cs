using AgrifoodManagement.Util.Models;
using MediatR;

namespace AgrifoodManagement.Business.Queries.Report
{
    public record GetOrderStatusDistributionQuery : IRequest<List<OrderStatusShareDto>>;
}
