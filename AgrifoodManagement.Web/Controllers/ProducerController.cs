using AgrifoodManagement.Business.Queries.Forum;
using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Web.Mappers;
using AgrifoodManagement.Web.Models;
using AgrifoodManagement.Web.Models.Forum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Seller")]
    public class ProducerController : BaseUserController
    {
        public ProducerController(IMediator mediator) : base(mediator) { }

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

        public async Task<IActionResult> StocksAsync()
        {
            SetSidebar("2");

            var products = await _mediator.Send(new GetProductStocksQuery());
            var stocks = ProductViewModelMapper.Map(products);

            return View(stocks);
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
            var productCategories = await _mediator.Send(new GetChildCategoriesQuery());
            SetSidebar("6");
            ViewBag.ProductCategories = productCategories;

            var productDtos = await _mediator.Send(new GetUserProductsQuery());

            var viewModel = productDtos.Select(p => new UpsertProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.CurrentPrice,
                Quantity = p.Quantity,
                UnitOfMeasurement = p.UnitOfMeasurement,
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
            var productCategories = await _mediator.Send(new GetProductCategoriesQuery());
            SetSidebar("6");
            ViewBag.ProductCategories = productCategories;

            UpsertProductViewModel viewModel = new UpsertProductViewModel();

            if (id.HasValue)
            {
                var product = await _mediator.Send(new GetProductByIdQuery { Id = id.Value });
                if (product == null)
                {
                    return NotFound();
                }

                viewModel = new UpsertProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.OriginalPrice,
                    Quantity = product.Quantity,
                    UnitOfMeasurement = product.UnitOfMeasurement,
                    ExpirationDate = product.ExpirationDate,
                    Category = product.CategoryId,
                    CategoryName = product.CategoryName,
                    PhotoUrls = product.PhotoUrls
                };
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ForumAsync()
        {
            SetSidebar("7");

            var topicDtos = await _mediator.Send(new GetForumTopicsQuery());

            var viewModel = topicDtos.Select(dto => new TopicViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Text = dto.Text,
                CreatedAt = dto.CreatedAt,
                Category = dto.Category,
                Author = new UserViewModel
                {
                    Name = dto.Author.Name,
                    AvatarUrl = dto.Author.AvatarUrl
                },
                LatestReplyAuthor = "@" + dto.LatestReplyAuthor,
                LatestReplyTimeAgo = GetTimeAgo(dto.LatestReplyTime),
                CommentsCount = dto.CommentsCount,
                TopCommenters = dto.TopCommenters.Select(u => new UserViewModel
                {
                    Name = u.Name,
                    AvatarUrl = u.AvatarUrl
                }).ToList(),
                Comments = dto.Comments.Select(c => new CommentViewModel
                {
                    Author = new UserViewModel
                    {
                        Name = c.Author.Name,
                        AvatarUrl = c.Author.AvatarUrl
                    },
                    Text = c.Text,
                    TimeAgo = GetTimeAgo(c.CreatedAt)
                }).ToList()
            }).ToList();

            return View(viewModel);
        }

        private List<SidebarViewModel> GetSidebarItems()
        {
            return new List<SidebarViewModel>
            {
                new SidebarViewModel { Id = "1", Name = "Dashboard", IconCss = "bi bi-speedometer2", Url = "/Producer/Dashboard", IsPro = false },
                new SidebarViewModel { Id = "2", Name = "Stock Management", IconCss = "bi bi-box-seam", Url = "/Producer/Stocks", IsPro = false },
                new SidebarViewModel { Id = "3", Name = "Orders & Transactions", IconCss = "bi bi-cart3", Url = "/Producer/Orders", IsPro = false },
                new SidebarViewModel { Id = "4", Name = "Reports & Analytics", IconCss = "bi bi-graph-up", Url = "/Producer/Reports", IsPro = true },
                new SidebarViewModel { Id = "5", Name = "Demand Forecasting", IconCss = "bi bi-calendar-check", Url = "/Producer/Forecasting", IsPro = true },
                new SidebarViewModel { Id = "6", Name = "Announcements", IconCss = "bi bi-megaphone", Url = "/Producer/Announcements", IsPro = false },
                new SidebarViewModel { Id = "7", Name = "Forum", IconCss = "bi bi-chat-dots", Url = "/Producer/Forum", IsPro = true },
            };
        }

        private string GetTimeAgo(DateTime? time)
        {
            if (time == null) return "unknown";
            var span = DateTime.UtcNow - time.Value;
            if (span.TotalMinutes < 1) return "just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} minutes ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours} hours ago";
            return $"{(int)span.TotalDays} days ago";
        }
    }
}
