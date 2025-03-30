using AgrifoodManagement.Business.Queries.Account;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Account
{
    public class ProductCategoriesQueryHandler : IRequestHandler<ProductCategoriesQuery, List<CategoryNode>>
    {
        private readonly IApplicationDbContext _context;

        public ProductCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryNode>> Handle(ProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.ProductCategories
                .Include(c => c.SubCategories)
                .ToListAsync(cancellationToken);

            var categoryNodes = categories.Select(c => new CategoryNode
            {
                Id = c.Id,
                Category = c.Name,
                ParentId = c.ParentCategoryId,
                HasChildren = c.SubCategories != null && c.SubCategories.Any(),
                Enabled = c.SubCategories != null && c.SubCategories.Any() ? false : true
            }).ToList();

            return categoryNodes;
        }
    }
}
