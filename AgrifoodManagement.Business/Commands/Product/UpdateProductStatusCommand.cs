using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public record UpdateProductStatusCommand(Guid ProductId, AnnouncementStatus NewStatus) : IRequest<bool>;
}
