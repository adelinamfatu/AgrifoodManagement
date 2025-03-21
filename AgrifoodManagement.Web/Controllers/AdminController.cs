using AgrifoodManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var sidebarItems = new List<SidebarItem>
            {
                new SidebarItem { Id = "1", Name = "Dashboard", Url = Url.Action("Dashboard", "Admin") },
                new SidebarItem { Id = "2", Name = "Reports", Url = Url.Action("Reports", "Admin") },
                new SidebarItem { Id = "3", Name = "Settings", Url = Url.Action("Reports", "Admin") }
            };

            ViewBag.SidebarItems = sidebarItems;

            return View();
        }

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
