using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Order
{
    public class UpdateCartItemQuantityCommand : IRequest<Unit>
    {
        public string BuyerEmail { get; set; } = null!;
        public Guid OrderDetailId { get; set; }
        public int NewQuantity { get; set; }
    }
}
