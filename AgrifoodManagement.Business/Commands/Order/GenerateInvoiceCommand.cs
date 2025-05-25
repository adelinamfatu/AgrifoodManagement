using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Order
{
    public class GenerateInvoiceCommand : IRequest<byte[]>
    {
        public Guid OrderId { get; }
        public GenerateInvoiceCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
