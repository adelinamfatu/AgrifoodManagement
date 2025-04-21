using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public class UpdateProductStockCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public double Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
