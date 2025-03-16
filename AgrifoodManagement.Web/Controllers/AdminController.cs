using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }
    }
}
