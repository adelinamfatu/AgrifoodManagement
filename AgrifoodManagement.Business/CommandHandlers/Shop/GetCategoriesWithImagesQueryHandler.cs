using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Shop
{
    public class GetCategoriesWithImagesQueryHandler : IRequestHandler<GetCategoriesWithImagesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoriesWithImagesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesWithImagesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.ProductCategories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var dtoLookup = categories.ToDictionary(
                x => x.Id,
                x => new CategoryDto
                {
                    Id = x.Id,
                    Category = x.Name,
                    ParentId = x.ParentCategoryId,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl
                });

            var result = new List<CategoryDto>();

            foreach (var cat in categories)
            {
                var dto = dtoLookup[cat.Id];
                if (cat.ParentCategoryId.HasValue && dtoLookup.ContainsKey(cat.ParentCategoryId.Value))
                {
                    dtoLookup[cat.ParentCategoryId.Value].Children.Add(dto);
                }
                else
                {
                    result.Add(dto);
                }
            }

            return result;
        }
    }
}
