using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    public class ConsumerController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Blogs()
        {
            return View();
        }
    }
}
