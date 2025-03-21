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
                new SidebarItem { Id = "1", Name = "Dashboard", IconCss = "bi bi-speedometer2", Url = "/Home/Dashboard" },
                new SidebarItem { Id = "2", Name = "Stock Management", IconCss = "bi bi-box-seam", Url = Url.Action("Stocks", "Admin") },
                new SidebarItem { Id = "3", Name = "Orders & Transactions", IconCss = "bi bi-cart3", Url = Url.Action("Orders", "Admin") },
                new SidebarItem { Id = "4", Name = "Reports & Analytics", IconCss = "bi bi-graph-up", Url = "/Home/Reports" },
                new SidebarItem { Id = "5", Name = "Demand Forecasting", IconCss = "bi bi-calendar-check", Url = Url.Action("Forecasting", "Admin") },
                new SidebarItem { Id = "6", Name = "Announcements", IconCss = "bi bi-megaphone", Url = Url.Action("Announcements", "Admin") },
                new SidebarItem { Id = "7", Name = "Forum", IconCss = "bi bi-chat-dots", Url = Url.Action("Forum", "Admin") },
                new SidebarItem { Id = "8", Name = "Settings", IconCss = "bi bi-gear", Url = Url.Action("Settings", "Admin") }
            };

            ViewBag.SidebarItems = sidebarItems;

            return View();
        }

        public IActionResult Dashboard()
        {
            return PartialView();
        }

        public IActionResult Reports()
        {
            return PartialView();
        }
    }
}
