using AgrifoodManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    public class AccountController : Controller
    {
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
        public IActionResult Register(RegisterViewModel model)
        {
            return View("Index", model);
        }
    }
}
