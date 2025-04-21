using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var p = await _context.Products.FindAsync(request.Id);
            if (p == null)
                throw new KeyNotFoundException($"Product '{request.Id}' not found.");

            p.Name = request.Name;
            p.Description = request.Description;
            p.UnitOfMeasurement = request.UnitOfMeasurement;
            p.ExpirationDate = request.ExpirationDate;
            p.ProductCategoryId = request.ProductCategoryId;

            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
