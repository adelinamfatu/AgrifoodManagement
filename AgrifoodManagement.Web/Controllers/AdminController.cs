using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Business.Queries.Account;
using AgrifoodManagement.Util;
using AgrifoodManagement.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Helper method to set the sidebar items and active item.
        /// </summary>
        private void SetSidebar(string activeItemId)
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = activeItemId;
        }

        public IActionResult Dashboard()
        {
            SetSidebar("1");
            return View();
        }

        public IActionResult Stocks()
        {
            SetSidebar("2");
            return View();
        }

        public IActionResult Orders()
        {
            SetSidebar("3");
            return View();
        }

        public IActionResult Reports()
        {
            SetSidebar("4");
            return View();
        }

        public IActionResult Forecasting()
        {
            SetSidebar("5");
            return View();
        }

        public async Task<IActionResult> AnnouncementsAsync()
        {
            var productCategories = await _mediator.Send(new ChildCategoriesQuery());
            SetSidebar("7");
            ViewBag.ProductCategories = productCategories;

            var viewModel = new ProductViewModel
            {
                Products = new List<ProductViewModel>
                {
                    new ProductViewModel
                    {
                        Name = "Organic Apples",
                        Description = "Fresh organic apples from our orchard. Pesticide-free and harvested this week.",
                        Price = 2.99m,
                        Quantity = 150.5,
                        UnitOfMeasurement = "kg",
                        ExpirationDate = DateTime.Now.AddDays(14),
                        Location = "North Field",
                        Category = "Fruits",
                        ViewCount = 45,
                        InquiryCount = 12,
                        DemandForecast = "High",
                        EstimatedMarketPrice = 3.45m,
                        IsArchived = false
                    },
                    new ProductViewModel
                    {
                        Name = "Winter Wheat",
                        Description = "Locally grown winter wheat, perfect for milling and baking. Chemical-free farming methods.",
                        Price = 1.75m,
                        Quantity = 500,
                        UnitOfMeasurement = "kg",
                        ExpirationDate = DateTime.Now.AddDays(60),
                        Location = "South Field",
                        Category = "Grains",
                        ViewCount = 28,
                        InquiryCount = 3,
                        DemandForecast = "Medium",
                        EstimatedMarketPrice = 1.85m,
                        IsArchived = false
                    },
                    new ProductViewModel
                    {
                        Name = "Fresh Tomatoes",
                        Description = "Vine-ripened tomatoes, picked at peak ripeness. Great for salads and sauces.",
                        Price = 3.49m,
                        Quantity = 75,
                        UnitOfMeasurement = "kg",
                        ExpirationDate = DateTime.Now.AddDays(5),
                        Location = "Greenhouse 2",
                        Category = "Vegetables",
                        ViewCount = 67,
                        InquiryCount = 15,
                        DemandForecast = "High",
                        EstimatedMarketPrice = 3.99m,
                        IsArchived = false
                    },
                    new ProductViewModel
                    {
                        Name = "Organic Milk",
                        Description = "Fresh organic milk from our grass-fed cows. Pasteurized but not homogenized.",
                        Price = 3.99m,
                        Quantity = 100,
                        UnitOfMeasurement = "liter",
                        ExpirationDate = DateTime.Now.AddDays(7),
                        Location = "Dairy Barn",
                        Category = "Dairy",
                        ViewCount = 38,
                        InquiryCount = 8,
                        DemandForecast = "Medium",
                        EstimatedMarketPrice = 4.25m,
                        IsArchived = false
                    }
                }
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AddAnnouncementAsync()
        {
            var productCategories = await _mediator.Send(new ProductCategoriesQuery());
            SetSidebar("6");
            ViewBag.ProductCategories = productCategories;

            return View();
        }

        public IActionResult Forum()
        {
            SetSidebar("7");
            return View();
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

                var command = new UploadUserPhotoCommand
                {
                    Photo = photo
                };

                var result = await _mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, imageUrl = "/images/avatar.jpg" });
                }

                return Json(new { success = false, message = result.Error });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
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
