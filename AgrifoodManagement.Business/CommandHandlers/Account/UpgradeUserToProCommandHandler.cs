using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AgrifoodManagement.Business.CommandHandlers.Account
{
    public class UpgradeUserToProCommandHandler : IRequestHandler<UpgradeUserToProCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public UpgradeUserToProCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpgradeUserToProCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.UserEmail, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.IsPro = true;

            _context.ExtendedProperties.Add(new ExtendedProperty
            {
                ID = Guid.NewGuid(),
                EntityType = "User",
                EntityId = user.Id,
                Key = "ProUpgradeDate",
                Value = request.TransactionDate.ToString("o")
            });

            _context.ExtendedProperties.Add(new ExtendedProperty
            {
                ID = Guid.NewGuid(),
                EntityType = "User",
                EntityId = user.Id,
                Key = "ProUpgradeValue",
                Value = request.TransactionValue.HasValue
                    ? request.TransactionValue.Value.ToString("F2", CultureInfo.InvariantCulture)
                    : "0.00"
            });

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
