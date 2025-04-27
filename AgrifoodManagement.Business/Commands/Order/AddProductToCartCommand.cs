using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Order
{
    public class AddProductToCartCommand : IRequest<Guid>
    {
        public string BuyerEmail { get; set; } = null!;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
