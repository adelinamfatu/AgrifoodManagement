using AgrifoodManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = "1";

            return View();
        }

        public IActionResult Stocks()
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = "2";

            return View();
        }

        public IActionResult Orders()
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = "3";

            return View();
        }

        public IActionResult Reports()
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = "4";

            return View();
        }

        public IActionResult Forecasting()
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = "5";

            return View();
        }

        public IActionResult Announcements()
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = "6";

            return View();
        }

        public IActionResult Forum()
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = "7";

            return View();
        }

        private List<SidebarItem> GetSidebarItems()
        {
            return new List<SidebarItem>
            {
                new SidebarItem { Id = "1", Name = "Dashboard", IconCss = "bi bi-speedometer2", Url = "/Admin/Dashboard" },
                new SidebarItem { Id = "2", Name = "Stock Management", IconCss = "bi bi-box-seam", Url = "/Admin/Stocks" },
                new SidebarItem { Id = "3", Name = "Orders & Transactions", IconCss = "bi bi-cart3", Url = "/Admin/Orders" },
                new SidebarItem { Id = "4", Name = "Reports & Analytics", IconCss = "bi bi-graph-up", Url = "/Admin/Reports" },
                new SidebarItem { Id = "5", Name = "Demand Forecasting", IconCss = "bi bi-calendar-check", Url = "/Admin/Forecasting" },
                new SidebarItem { Id = "6", Name = "Announcements", IconCss = "bi bi-megaphone", Url = "/Admin/Announcements" },
                new SidebarItem { Id = "7", Name = "Forum", IconCss = "bi bi-chat-dots", Url = "/Admin/Forum" },
            };
        }
    }
}
