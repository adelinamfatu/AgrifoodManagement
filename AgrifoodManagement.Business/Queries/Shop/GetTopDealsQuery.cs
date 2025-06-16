using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Queries.Shop
{
    public class GetTopDealsQuery : IRequest<Result<List<ProductDto>>>
    {
        public int TakeCount { get; }

        public GetTopDealsQuery(int takeCount = 15)
        {
            TakeCount = takeCount;
        }
    }
}
