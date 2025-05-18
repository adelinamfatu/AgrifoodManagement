using AgrifoodManagement.Util.Models;
using MediatR;

namespace AgrifoodManagement.Business.Queries.Account
{
    public record GetSellerLocationsQuery() : IRequest<List<LocationDto>>;
}
