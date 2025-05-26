using AgrifoodManagement.Business.Queries.Account;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Locator
{
    public class GetSellerLocationsQueryHandler : IRequestHandler<GetSellerLocationsQuery, List<LocationDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetSellerLocationsQueryHandler(ApplicationDbContext context)
            => _context = context;

        public async Task<List<LocationDto>> Handle(GetSellerLocationsQuery req, CancellationToken ct)
        {
            return await _context.Users
                .Where(u => u.UserType == UserType.Seller
                         && u.Latitude.HasValue
                         && u.Longitude.HasValue)
                .Select(u => new LocationDto
                {
                    Latitude = u.Latitude!.Value,
                    Longitude = u.Longitude!.Value,
                    Name = $"{u.FirstName} {u.LastName}"
                })
                .ToListAsync(ct);
        }
    }
}
