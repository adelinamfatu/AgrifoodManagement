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
    public class GetProductsPerPageQuery : IRequest<Result<PagedResult<ProductDto>>>
    {
        public string UserEmail { get; set; }
        public int Page { get; }
        public int PageSize { get; }

        public GetProductsPerPageQuery(string userEmail, int page, int pageSize)
        {
            UserEmail = userEmail;
            Page = page;
            PageSize = pageSize;
        }
    }
}
