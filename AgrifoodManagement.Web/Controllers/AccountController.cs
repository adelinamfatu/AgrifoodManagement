using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
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

            var registerCommand = new RegisterUserCommand
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserType = model.UserType,
                PhoneNumber = model.PhoneNumber
            };

            var registerResult = await _mediator.Send(registerCommand);

            if (!registerResult.IsSuccess)
            {
                ModelState.AddModelError("", registerResult.Error);
                return View("Index", model);
            }

            // Automatically log in the user after successful registration
            var loginCommand = new LoginUserCommand
            {
                Email = model.Email,
                Password = model.Password
            };

            var loginResult = await _mediator.Send(loginCommand);

            if (!loginResult.IsSuccess)
            {
                return Unauthorized(loginResult.Error);
            }

            // Set the JWT token as a secure cookie
            Response.Cookies.Append("AuthToken", loginResult.Value.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = loginResult.Value.Expiration
            });

            switch (loginResult.Value.UserType)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Remove the AuthToken cookie
            if (Request.Cookies.ContainsKey("AuthToken"))
            {
                Response.Cookies.Delete("AuthToken");
            }

            // Redirect back to login
            return RedirectToAction("Auth", "Account");
        }
    }
}
