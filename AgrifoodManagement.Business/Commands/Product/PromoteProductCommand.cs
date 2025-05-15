using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public record PromoteProductCommand(Guid ProductId, string StripeIntentId) : IRequest<bool>;
}
