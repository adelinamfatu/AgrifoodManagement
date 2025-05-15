using AgrifoodManagement.Business.Queries.Forum;
using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Business.Queries.Report;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Mappers;
using AgrifoodManagement.Web.Models;
using AgrifoodManagement.Web.Models.Forum;
using AgrifoodManagement.Web.Models.Report;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        public async Task<IActionResult> AnnouncementsAsync([FromQuery] string filter = "active")
        {
            SetSidebar("1");

            var productCategories = await _mediator.Send(new GetChildCategoriesQuery());
            ViewBag.ProductCategories = productCategories;

            var allDtos = await _mediator.Send(new GetUserProductsQuery());

            IEnumerable<ProductDto> filtered = filter switch
            {
                "active" => allDtos
                                  .Where(d => d.AnnouncementStatus == AnnouncementStatus.Published),
                "expiring" => allDtos
                                  .Where(d => (d.ExpirationDate - DateTime.UtcNow).TotalDays < 14),
                "lowStock" => allDtos
                                  .Where(d => d.Quantity < 21),
                "archived" => allDtos
                                  .Where(d => d.AnnouncementStatus == AnnouncementStatus.Archived
                                           || d.AnnouncementStatus == AnnouncementStatus.Expired),
                _ => allDtos
            };

            var viewModel = UpsertProductViewModelMapper.MapList(filtered);

            ViewBag.CurrentFilter = filter;

            return View(viewModel);
        }

        public async Task<IActionResult> ProductAsync(Guid? id)
        {
            SetSidebar("1");

            var productCategories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.ProductCategories = productCategories;

            UpsertProductViewModel viewModel;

            if (id.HasValue)
            {
                var dto = await _mediator.Send(new GetProductByIdQuery { Id = id.Value });
                if (dto == null) return NotFound();

                viewModel = UpsertProductViewModelMapper.MapForDetail(dto);
            }
            else
            {
                viewModel = new UpsertProductViewModel();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> StocksAsync()
        {
            SetSidebar("2");

            var products = await _mediator.Send(new GetProductStocksQuery());
            var stocks = ProductViewModelMapper.Map(products);

            return View(stocks);
        }

        public async Task<IActionResult> OrdersAsync()
        {
            SetSidebar("3");

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Auth", "Account");

            var dto = await _mediator.Send(new GetSellerOrdersQuery(email));
            var viewModel = OrderHistoryViewModelMapper.Map(dto);
            return View(viewModel);
        }

        public async Task<IActionResult> ReportsAsync()
        {
            SetSidebar("4");

            var pieData = await _mediator.Send(new GetCategoryShareQuery());
            var columnData = await _mediator.Send(new GetQuarterlySalesQuery());
            var splineData = await _mediator.Send(new GetMonthlySalesQuery(6));

            var viewModel = new ReportsViewModel
            {
                CellSpacing = new double[] { 10, 10 },

                PieData = pieData
                         .Select(x => new PieData
                         {
                             Product = x.Category,
                             Percentage = x.Percentage
                         }).ToList(),

                ColumnChartData = columnData
                         .Select(x => new ColumnData
                         {
                             Period = x.Period,
                             OnlinePercentage = x.Sales 
                         }).ToList(),

                SplineChartData = splineData
                         .Select(x => new SplineData
                         {
                             Period = x.Period,
                             OnlinePercentage = x.Sales
                         }).ToList(),

                Palettes = new[] { "#2485FA", "#FEC200", "#28A745", "#DC3545" }
            };

            return View(viewModel);
        }

        public IActionResult Forecasting()
        {
            SetSidebar("5");
            return View();
        }

        public async Task<IActionResult> ForumAsync()
        {
            SetSidebar("6");

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
                new SidebarViewModel { Id = "1", Name = "Announcements", IconCss = "bi bi-megaphone", Url = "/Producer/Announcements", IsPro = false },
                new SidebarViewModel { Id = "2", Name = "Stock Management", IconCss = "bi bi-box-seam", Url = "/Producer/Stocks", IsPro = false },
                new SidebarViewModel { Id = "3", Name = "Orders & Transactions", IconCss = "bi bi-cart3", Url = "/Producer/Orders", IsPro = false },
                new SidebarViewModel { Id = "4", Name = "Reports & Analytics", IconCss = "bi bi-graph-up", Url = "/Producer/Reports", IsPro = true },
                new SidebarViewModel { Id = "5", Name = "Demand Forecasting", IconCss = "bi bi-calendar-check", Url = "/Producer/Forecasting", IsPro = true },
                new SidebarViewModel { Id = "6", Name = "Forum", IconCss = "bi bi-chat-dots", Url = "/Producer/Forum", IsPro = true },
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
