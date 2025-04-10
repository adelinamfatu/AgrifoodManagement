using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgrifoodManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AccountController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            var command = new LoginUserCommand
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return Unauthorized(result.Error);
            }

            // Set the token in a cookie
            Response.Cookies.Append("AuthToken", result.Value.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.Value.Expiration
            });

            switch (result.Value.UserType)
            {
                case UserType.Seller:
                    return Redirect("/Producer/Dashboard");
                case UserType.Buyer:
                    return Redirect("/Consumer/Home");
                default:
                    return Redirect("/");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var command = new RegisterUserCommand
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserType = model.UserType,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Error);
                return View("Index", model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            try
            {
                if (photo == null || photo.Length == 0)
                {
                    return Json(new { success = false, message = "No file was uploaded" });
                }

                var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmailClaim))
                {
                    return Json(new { success = false, message = "User not found" });
                }

                var command = new UploadUserPhotoCommand
                {
                    Photo = photo,
                    PhotoFolder = PhotoFolder.Users,
                    UserEmail = userEmailClaim
                };

                var result = await _mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, imageUrl = result.Value });
                }

                return Json(new { success = false, message = result.Error });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }
    }
}
