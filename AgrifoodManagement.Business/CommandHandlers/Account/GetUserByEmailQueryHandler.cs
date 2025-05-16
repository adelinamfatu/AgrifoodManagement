using AgrifoodManagement.Business.Queries.Account;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Account
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        private readonly IApplicationDbContext _context;

        public GetUserByEmailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(u => u.Email == request.Email)
                .Select(u => new {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.IsPro,
                    u.Avatar
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            var rawDate = await _context.ExtendedProperties
                .Where(ep =>
                    ep.EntityType == "User" &&
                    ep.EntityId == user.Id &&
                    ep.Key == "ProUpgradeDate")
                .Select(ep => ep.Value)
                .FirstOrDefaultAsync(cancellationToken);

            int daysLeft = 0;
            if (rawDate != null
                && DateTime.TryParse(
                     rawDate,
                     CultureInfo.InvariantCulture,
                     DateTimeStyles.RoundtripKind,
                     out var proStart))
            {
                var expiry = proStart.AddDays(30);
                var span = expiry.Date - DateTime.UtcNow.Date;
                daysLeft = Math.Max(0, span.Days);
            }

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsPro = user.IsPro,
                AvatarUrl = user.Avatar ?? "/images/avatar-placeholder.png",
                DaysLeft = daysLeft
            };
        }
    }
}
