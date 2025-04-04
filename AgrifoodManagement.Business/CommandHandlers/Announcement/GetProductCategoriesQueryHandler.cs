using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, List<CategoryNode>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryNode>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
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
