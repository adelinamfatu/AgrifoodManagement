using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Order
{
    public record AddReviewCommand(string UserEmail, Guid ProductId, int Rating, string Comment) : IRequest<bool>;
}
