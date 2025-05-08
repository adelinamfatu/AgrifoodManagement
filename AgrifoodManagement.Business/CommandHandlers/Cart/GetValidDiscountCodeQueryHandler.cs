using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class GetValidDiscountCodeQueryHandler : IRequestHandler<GetValidDiscountCodeQuery, DiscountCodeDto?>
    {
        private readonly ApplicationDbContext _context;

        public GetValidDiscountCodeQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiscountCodeDto?> Handle(GetValidDiscountCodeQuery request, CancellationToken cancellationToken)
        {
            var code = request.Code.Trim().ToUpper();

            var discount = await _context.DiscountCodes
                .Where(dc => dc.Code.ToUpper() == code &&
                             (dc.ExpiresAt == null || dc.ExpiresAt > DateTime.UtcNow))
                .FirstOrDefaultAsync(cancellationToken);

            if (discount == null) return null;

            return new DiscountCodeDto
            {
                Code = discount.Code,
                Type = discount.Type,
                Value = discount.Value
            };
        }
    }
}
