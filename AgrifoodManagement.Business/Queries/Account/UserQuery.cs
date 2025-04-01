using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Util.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Queries.Account
{
    public class UserQuery : IRequest<UserDto>
    {
        public string Email { get; }

        public UserQuery(string email)
        {
            Email = email;
        }
    }
}
