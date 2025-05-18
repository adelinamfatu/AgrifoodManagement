using AgrifoodManagement.Util.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Queries.Account
{
    public class GetUserSettingsQuery : IRequest<UserSettingsDto>
    {
        public string Email { get; }
        public GetUserSettingsQuery(string email) => Email = email;
    }
}
