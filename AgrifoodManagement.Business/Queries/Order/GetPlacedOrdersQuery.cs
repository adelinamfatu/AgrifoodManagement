using AgrifoodManagement.Util.ValueObjects;
using MediatR;

namespace AgrifoodManagement.Business.Queries.Order
{
    public record GetProcessedOrdersQuery(string BuyerEmail) : IRequest<List<OrderTreeDto>>;
}
