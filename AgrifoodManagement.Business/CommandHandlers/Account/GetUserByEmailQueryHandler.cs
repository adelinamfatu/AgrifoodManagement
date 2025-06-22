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

            var allDates = await _context.ExtendedProperties
                .Where(ep =>
                    ep.EntityType == "User" &&
                    ep.EntityId == user.Id &&
                    ep.Key == "ProUpgradeDate")
                .Select(ep => ep.Value)
                .ToListAsync(cancellationToken);

            DateTime? proStart = allDates
                .Where(v => DateTime.TryParse(
                                v,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.RoundtripKind,
                                out _))
                .Select(v => DateTime.Parse(
                                v,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.RoundtripKind))
                .OrderByDescending(d => d)
                .FirstOrDefault();

            int daysLeft = 0;
            if (proStart.HasValue)
            {
                var expiry = proStart.Value.AddDays(30);
                daysLeft = Math.Max(0, (expiry.Date - DateTime.UtcNow.Date).Days);
            }

            if (user.IsPro && daysLeft == 0)
            {
                var userEntity = await _context.Users.FindAsync(new object[] { user.Id }, cancellationToken);
                if (userEntity is not null)
                {
                    userEntity.IsPro = false;
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsPro = user.IsPro && daysLeft > 0,
                AvatarUrl = user.Avatar ?? "/images/avatar-placeholder.png",
                DaysLeft = daysLeft
            };
        }
    }
}
