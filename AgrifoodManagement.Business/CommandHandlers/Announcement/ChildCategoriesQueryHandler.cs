using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class ChildCategoriesQueryHandler : IRequestHandler<ChildCategoriesQuery, List<CategoryNode>>
    {
        private readonly ApplicationDbContext _context;

        public ChildCategoriesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryNode>> Handle(ChildCategoriesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories;

            if (request.ParentCategoryId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == request.ParentCategoryId.Value);
            }
            else
            {
                query = query.Where(c => c.ParentCategoryId != null);
            }

            var categories = await query.ToListAsync(cancellationToken);

            var categoryNodes = categories.Select(c => new CategoryNode
            {
                Id = c.Id,
                Category = c.Name,
                ParentId = c.ParentCategoryId,
                HasChildren = c.SubCategories != null && c.SubCategories.Any()
            }).ToList();

            return categoryNodes;
        }
    }
}
