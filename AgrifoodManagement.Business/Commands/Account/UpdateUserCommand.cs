using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Account
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string Email { get; init; } = "";
        public string? Password { get; init; }
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string PhoneNumber { get; init; } = "";
        public string DeliveryAddress { get; init; } = "";
    }
}
