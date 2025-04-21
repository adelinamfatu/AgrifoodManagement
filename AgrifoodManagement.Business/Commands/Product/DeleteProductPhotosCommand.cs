using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public class DeleteProductPhotosCommand : IRequest<Unit>
    {
        public Guid ProductId { get; set; }
    }
}
