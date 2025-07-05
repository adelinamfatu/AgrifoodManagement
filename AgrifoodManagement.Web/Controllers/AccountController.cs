using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models.Auth;
using AgrifoodManagement.Web.Models.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IStripeCheckoutService _stripeCheckoutService;

        public AccountController(IMediator mediator, IStripeCheckoutService stripeCheckoutService)
        {
            _mediator = mediator;
            _stripeCheckoutService = stripeCheckoutService;
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
                TempData["Error"] = result.Error;
                return RedirectToAction("Auth");
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
                    return Redirect("/Producer/Announcements");
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
                    return Redirect("/Producer/Stocks");
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
        public async Task<IActionResult> UpdateProfile(UpdateUserViewModel viewModel, IFormFile SignatureFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value.Errors
                                          .Select(err => err.ErrorMessage))
                    .ToArray();

                return BadRequest(new { errors });
            }

            var cmd = new UpdateUserCommand
            {
                UserId = viewModel.UserId,
                Email = viewModel.Email,
                Password = viewModel.Password,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                DeliveryAddress = viewModel.Address,
                SignatureFile = SignatureFile
            };

            await _mediator.Send(cmd);
            return Ok(new { success = true, message = "Profile updated!" });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProCheckoutSession()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var items = new[]
            {
                new StripeLineItemDto {
                    Name = "Harvestica PRO Subscription",
                    Description = "Unlock PRO features",
                    UnitAmount = 4999,
                    Currency = "ron",
                    Quantity = 1
                }
            };

            var domain = $"{Request.Scheme}://{Request.Host}";
            var successUrl = $"{domain}/Account/CompletePro?session_id={{CHECKOUT_SESSION_ID}}";
            var cancelUrl = $"{domain}/Producer/Announcements";

            var result = await _stripeCheckoutService.CreateCheckoutSessionAsync(
                lineItems: items,
                successUrl: successUrl,
                cancelUrl: cancelUrl,
                metadata: new Dictionary<string, string> { ["UserEmail"] = email }
            );

            if (!result.Success)
                return BadRequest(new { success = false, error = result.ErrorMessage });

            return Ok(new
            {
                success = true,
                sessionId = result.SessionId,
                publishableKey = result.PublishableKey
            });
        }

        [HttpGet]
        public async Task<IActionResult> CompletePro(string session_id)
        {
            var service = new Stripe.Checkout.SessionService();
            var session = await service.GetAsync(session_id);
            if (session.PaymentStatus != "paid")
            {
                TempData["Error"] = "Payment not completed.";
                return RedirectToAction("Announcements", "Producer");
            }

            if (!session.Metadata.TryGetValue("UserEmail", out var userEmail))
            {
                return BadRequest("Invalid session metadata.");
            }

            await _mediator.Send(new UpgradeUserToProCommand(
                userEmail,
                session.Created,
                session.AmountTotal / 100m
            ));

            return RedirectToAction("Announcements", "Producer");
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
