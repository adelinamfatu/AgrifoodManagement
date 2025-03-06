using AgrifoodManagement.Business.Commands;
using AgrifoodManagement.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "test@example.com" && model.Password == "password")
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
            }

            return View("Index", model);
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
                FirstName = "Adelina",
                LastName = "Fatu",
                PhoneNumber = "555232322"
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Error);
                return View("Index", model);
            }

            return RedirectToAction("Login");
        }
    }
}
