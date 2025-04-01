using AgrifoodManagement.Util;
using AgrifoodManagement.Util.Models;
using MediatR;

namespace AgrifoodManagement.Business.Commands.Account
{
    public class LoginUserCommand : IRequest<Result<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
