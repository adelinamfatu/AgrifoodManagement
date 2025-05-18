using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public record ToggleWishlistItemCommand(string Email, Guid ProductId) : IRequest<bool>;
}
