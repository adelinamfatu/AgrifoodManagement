using AgrifoodManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Auth()
        {
            // Optionally, you can pass a new instance of LoginViewModel
            // to ensure that the login partial has a model to work with.
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        // This action handles the login form submission.
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Replace this with your actual authentication logic.
                // For example, verify the user's credentials from a database:
                // bool isValidUser = _authService.ValidateUser(model.Email, model.Password);
                // if (isValidUser) { ... }

                // For demonstration, let's assume a simple check:
                if (model.Email == "test@example.com" && model.Password == "password")
                {
                    // Login successful.
                    // For example, set authentication cookies, initialize session, etc.
                    // Then redirect to a dashboard or home page.
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    // Add a model error to display an error message on the login form.
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
            }

            // If the login failed, redisplay the main view.
            // The login partial will show validation errors.
            return View("Index", model);
        }

        // POST: /Account/Register
        // This action handles the registration form submission.
        [HttpPost]
        public IActionResult Register(string Name, string Email, string Password)
        {
            // TODO: Add your registration logic here.
            // For example:
            // 1. Validate the incoming data.
            // 2. Create a new user record in your data store.
            // 3. Optionally, log the user in automatically or send a confirmation email.

            // For demonstration, let's assume registration is always successful.
            // If registration fails, you might add errors to the ModelState and return the Index view.

            // Example (pseudo-code):
            // var result = _userService.CreateUser(Name, Email, Password);
            // if (!result.Succeeded)
            // {
            //     foreach (var error in result.Errors)
            //         ModelState.AddModelError(string.Empty, error.Description);
            //     return View("Index", new LoginViewModel());  // or create a view model that encapsulates both forms
            // }

            // Registration succeeded. Redirect to a dashboard or login page.
            return RedirectToAction("Dashboard", "Home");
        }
    }
}
