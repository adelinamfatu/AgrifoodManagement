using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Account
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IGeocodingService _geocoder;

        public UpdateUserCommandHandler(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, IGeocodingService geocodingService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _geocoder = geocodingService;
        }

        public async Task<Unit> Handle(UpdateUserCommand cmd, CancellationToken ct)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == cmd.UserId, ct);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.Email = cmd.Email;
            user.FirstName = cmd.FirstName;
            user.LastName = cmd.LastName;
            user.PhoneNumber = cmd.PhoneNumber;
            user.Address = cmd.DeliveryAddress;

            var (lat, lon) = await _geocoder.GeocodeAddressAsync(cmd.DeliveryAddress);
            user.Latitude = lat;
            user.Longitude = lon;

            if (!string.IsNullOrWhiteSpace(cmd.Password))
            {
                user.Password = _passwordHasher.HashPassword(null, cmd.Password);
            }

            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
