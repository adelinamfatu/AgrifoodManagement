using AgrifoodManagement.Util.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Queries.Product
{
    public class GetUserProductsQuery : IRequest<List<ProductDto>>
    {
        public string UserEmail { get; }
        public GetUserProductsQuery(string userEmail)
        {
            UserEmail = userEmail;
        }
    }
}
