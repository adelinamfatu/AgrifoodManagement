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

        // Filters
        public MeasurementUnit? UnitFilter { get; }
        public string PriceRange { get; }
        public string SortBy { get; }
        public int? CategoryId { get; }

        public GetProductsPerPageQuery(
            string userEmail,
            int page,
            int pageSize,
            MeasurementUnit? unitFilter,
            string priceRange,
            string sortBy,
            int? categoryId)
        {
            UserEmail = userEmail;
            Page = page;
            PageSize = pageSize;
            UnitFilter = unitFilter;
            PriceRange = priceRange;
            SortBy = sortBy;
            CategoryId = categoryId;
        }
    }
}
