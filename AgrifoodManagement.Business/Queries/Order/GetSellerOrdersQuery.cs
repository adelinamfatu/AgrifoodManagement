using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Queries.Order
{
    public record GetSellerOrdersQuery(string SellerEmail) : IRequest<List<OrderTreeDto>>;
}
