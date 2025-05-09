using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Order
{
    public class MarkOrderAsPaidCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
    }
}
