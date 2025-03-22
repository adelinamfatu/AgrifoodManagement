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

        private List<SidebarViewModel> GetSidebarItems()
        {
            return new List<SidebarViewModel>
            {
                new SidebarViewModel { Id = "1", Name = "Dashboard", IconCss = "bi bi-speedometer2", Url = "/Admin/Dashboard", IsPro = false },
                new SidebarViewModel { Id = "2", Name = "Stock Management", IconCss = "bi bi-box-seam", Url = "/Admin/Stocks", IsPro = false },
                new SidebarViewModel { Id = "3", Name = "Orders & Transactions", IconCss = "bi bi-cart3", Url = "/Admin/Orders", IsPro = false },
                new SidebarViewModel { Id = "4", Name = "Reports & Analytics", IconCss = "bi bi-graph-up", Url = "/Admin/Reports", IsPro = true },
                new SidebarViewModel { Id = "5", Name = "Demand Forecasting", IconCss = "bi bi-calendar-check", Url = "/Admin/Forecasting", IsPro = true },
                new SidebarViewModel { Id = "6", Name = "Announcements", IconCss = "bi bi-megaphone", Url = "/Admin/Announcements", IsPro = false },
                new SidebarViewModel { Id = "7", Name = "Forum", IconCss = "bi bi-chat-dots", Url = "/Admin/Forum", IsPro = true },
            };
        }
    }
}
