using AgrifoodManagement.Business.Queries.Account;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    public abstract class BaseAdminController : Controller
    {
        protected readonly IMediator _mediator;

        protected BaseAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.IsNullOrEmpty(email))
            {
                UserDto user = await _mediator.Send(new UserQuery(email));
                ViewBag.FullName = user != null ? $"{user.FirstName} {user.LastName}" : "Guest";
                ViewBag.AvatarUrl = user?.AvatarUrl != string.Empty ? user?.AvatarUrl 
                    : "/images/avatar-placeholder.png";
            }
            else
            {
                ViewBag.FullName = "Guest";
                ViewBag.AvatarUrl = "/images/default-avatar.png";
            }

            await next();
        }
    }
}
