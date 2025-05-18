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
    public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, UserSettingsDto>
    {
        private readonly IApplicationDbContext _context;

        public GetUserSettingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserSettingsDto> Handle(GetUserSettingsQuery req, CancellationToken token)
        {
            var user = await _context.Users
                .Where(x => x.Email == req.Email)
                .Select(x => new {
                    x.Id,
                    x.Email,
                    x.FirstName,
                    x.LastName,
                    x.Avatar,
                    x.UserType,
                    x.PhoneNumber,
                    x.Address,
                    x.Latitude,
                    x.Longitude
                })
                .FirstOrDefaultAsync(token);

            if (user == null)
                return null;

            return new UserSettingsDto
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar ?? "/images/avatar-placeholder.png",
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Latitude = user.Latitude,
                Longitude = user.Longitude
            };
        }
    }
}
