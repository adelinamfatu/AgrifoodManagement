using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Stock
{
    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductStockCommand cmd, CancellationToken ct)
        {
            var p = await _context.Products.FindAsync(cmd.Id);

            if (p == null)
                throw new KeyNotFoundException($"Product with id {cmd.Id} was not found.");

            var originalPrice = p.CurrentPrice ?? p.OriginalPrice;

            if (cmd.Quantity < 0)
                throw new ValidationException("Quantity cannot be negative.");

            if (cmd.CurrentPrice > originalPrice)
                throw new ValidationException("You cannot increase the price, only discount it.");

            p.Quantity = cmd.Quantity;
            p.CurrentPrice = cmd.CurrentPrice;
            await _context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
