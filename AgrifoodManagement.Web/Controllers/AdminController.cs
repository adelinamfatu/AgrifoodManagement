using AgrifoodManagement.Business.Queries.Account;
using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize]
    public class AdminController : BaseAdminController
    {
        public AdminController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Helper method to set the sidebar items and active item.
        /// </summary>
        private void SetSidebar(string activeItemId)
        {
            ViewBag.SidebarItems = GetSidebarItems();
            ViewBag.ActiveItemId = activeItemId;
        }

        public async Task<IActionResult> DashboardAsync()
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

            var productDtos = await _mediator.Send(new ProductsQuery());

            var viewModel = productDtos.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                UnitOfMeasurement = Enum.Parse<MeasurementUnit>(p.UnitOfMeasurement),
                ExpirationDate = p.ExpirationDate,
                Category = p.CategoryId,
                CategoryName = p.CategoryName,
                ViewCount = p.ViewCount,
                InquiryCount = p.InquiryCount,
                EstimatedMarketPrice = p.EstimatedMarketPrice,
                IsPromoted = p.IsPromoted,
                AnnouncementStatus = p.AnnouncementStatus,
                PhotoUrls = p.PhotoUrls
            }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> ProductAsync(Guid? id)
        {
            var productCategories = await _mediator.Send(new ProductCategoriesQuery());
            SetSidebar("6");
            ViewBag.ProductCategories = productCategories;

            ProductViewModel viewModel = new ProductViewModel();

            if (id.HasValue)
            {
                //var product = await _mediator.Send(new GetProductByIdQuery { Id = id.Value });
                //if (product == null)
                //{
                //    return NotFound();
                //}

                //viewModel = new ProductViewModel
                //{
                //    Id = product.Id,
                //    Name = product.Name,
                //    Description = product.Description,
                //    Price = product.Price,
                //    Quantity = product.Quantity,
                //    UnitOfMeasurement = product.UnitOfMeasurement,
                //    ExpirationDate = product.ExpirationDate,
                //    Category = product.ProductCategoryId
                //};
            }

            return View(viewModel);
        }

        public IActionResult Forum()
        {
            SetSidebar("7");
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
