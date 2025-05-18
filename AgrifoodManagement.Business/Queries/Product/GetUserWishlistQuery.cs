using AgrifoodManagement.Util.Models;
using MediatR;

namespace AgrifoodManagement.Business.Queries.Product
{
    public record GetUserWishlistQuery(string Email) : IRequest<List<WishlistItemDto>>;
}
