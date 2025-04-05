using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Web.Models;
using AgrifoodManagement.Web.Models.Forum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var productCategories = await _mediator.Send(new GetChildCategoriesQuery());
            SetSidebar("7");
            ViewBag.ProductCategories = productCategories;

            var productDtos = await _mediator.Send(new GetUserProductsQuery());

            var viewModel = productDtos.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
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

            ProductViewModel viewModel = new ProductViewModel();

            if (id.HasValue)
            {
                var product = await _mediator.Send(new GetProductByIdQuery { Id = id.Value });
                if (product == null)
                {
                    return NotFound();
                }

                viewModel = new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
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

        public IActionResult Forum()
        {
            SetSidebar("7");

            var users = new List<UserViewModel>
            {
                new UserViewModel { Id = 1, Name = "Harry Smith", AvatarUrl = "/images/avatars/user1.jpg" },
                new UserViewModel { Id = 2, Name = "Sarah Patel", AvatarUrl = "/images/avatars/user2.jpg" },
                new UserViewModel { Id = 3, Name = "Mike Johnson", AvatarUrl = "/images/avatars/user3.jpg" },
                new UserViewModel { Id = 4, Name = "David Miller", AvatarUrl = "/images/avatars/user4.jpg" },
                new UserViewModel { Id = 5, Name = "Jane Smith", AvatarUrl = "/images/avatars/user5.jpg" },
                new UserViewModel { Id = 6, Name = "Alex Chen", AvatarUrl = "/images/avatars/user6.jpg" },
                new UserViewModel { Id = 7, Name = "Taylor Wong", AvatarUrl = "/images/avatars/user7.jpg" },
                new UserViewModel { Id = 8, Name = "Morgan Lee", AvatarUrl = "/images/avatars/user8.jpg" }
            };

            // Create sample comments for first topic
            var topic1Comments = new List<CommentViewModel>
            {
                new CommentViewModel
                {
                    Id = 1,
                    Author = users[1],
                    Text = "Welcome to the forum, Harry! Great to have you join us. Looking forward to your contributions!",
                    TimeAgo = "3 minutes ago"
                },
                new CommentViewModel
                {
                    Id = 2,
                    Author = users[5],
                    Text = "Hi Harry! I'm also relatively new here. The community is really supportive and helpful.",
                    TimeAgo = "15 minutes ago"
                },
                new CommentViewModel
                {
                    Id = 3,
                    Author = users[3],
                    Text = "Hey there! What kind of projects are you interested in working on?",
                    TimeAgo = "1 hour ago"
                },
                new CommentViewModel
                {
                    Id = 4,
                    Author = users[4],
                    Text = "Welcome aboard! Don't forget to check out the resources section - lots of helpful links there.",
                    TimeAgo = "2 hours ago"
                }
            };

            // Create sample comments for second topic
            var topic2Comments = new List<CommentViewModel>
            {
                new CommentViewModel
                {
                    Id = 5,
                    Author = users[2],
                    Text = "Yes, you can definitely upgrade your existing membership. Just go to account settings and select the upgrade option.",
                    TimeAgo = "8 minutes ago"
                },
                new CommentViewModel
                {
                    Id = 6,
                    Author = users[6],
                    Text = "I upgraded mine last month. The process was seamless and they prorated the difference.",
                    TimeAgo = "45 minutes ago"
                },
                new CommentViewModel
                {
                    Id = 7,
                    Author = users[7],
                    Text = "There's usually a special discount if you upgrade before your current membership expires.",
                    TimeAgo = "1 day ago"
                }
            };

            // Create sample comments for third topic
            var topic3Comments = new List<CommentViewModel>
            {
                new CommentViewModel
                {
                    Id = 8,
                    Author = users[3],
                    Text = "That sounds like an interesting project! I'd love to see some of your design concepts.",
                    TimeAgo = "1 hour ago"
                },
                new CommentViewModel
                {
                    Id = 9,
                    Author = users[1],
                    Text = "I've worked on similar projects. Let me know if you need any feedback on your wireframes.",
                    TimeAgo = "3 hours ago"
                }
            };

            // Create sample comments for fourth topic
            var topic4Comments = new List<CommentViewModel>
            {
                new CommentViewModel
                {
                    Id = 10,
                    Author = users[4],
                    Text = "The booking flow looks good, but I think the search results page could use some work on the filter options.",
                    TimeAgo = "2 hours ago"
                },
                new CommentViewModel
                {
                    Id = 11,
                    Author = users[5],
                    Text = "I like the color scheme you've chosen. It gives a very professional feel to the app.",
                    TimeAgo = "5 hours ago"
                },
                new CommentViewModel
                {
                    Id = 12,
                    Author = users[2],
                    Text = "Have you considered adding a feature to save favorite routes? That would be really useful for frequent travelers.",
                    TimeAgo = "1 day ago"
                }
            };

            // Create sample topics
            var topics = new List<TopicViewModel>
            {
                new TopicViewModel
                {
                    Id = 1,
                    Title = "Introduce Yourself!",
                    Text = "Hey everyone, new member alert here! Thought I'd write a bit about myself and why I'm here. First of my name is Harry and I'm 31 years old! Massive fan of...",
                    Category = "Introductions",
                    Author = users[0],
                    LatestReplyAuthor = "@sarahpatel",
                    LatestReplyTimeAgo = "3 minutes ago",
                    CommentsCount = topic1Comments.Count,
                    TopCommenters = new List<UserViewModel> { users[1], users[3], users[4], users[5] },
                    Comments = topic1Comments
                },
                new TopicViewModel
                {
                    Id = 2,
                    Title = "The 12 month member programme",
                    Text = "This is long-winded! Quick question. If I already have a membership, can I upgrade it? Or should I wait until it ends?",
                    Category = "FAQ",
                    Author = users[6],
                    LatestReplyAuthor = "@mike",
                    LatestReplyTimeAgo = "8 minutes ago",
                    CommentsCount = topic2Comments.Count,
                    TopCommenters = new List<UserViewModel> { users[2], users[6], users[7], users[0] },
                    Comments = topic2Comments
                },
                new TopicViewModel
                {
                    Id = 3,
                    Title = "What are you working on?",
                    Text = "Right now I'm working on this fantastic client who are looking to re-design their forums. Currently we've just completed our research phase and moving into...",
                    Category = "Off-Topic",
                    Author = users[2],
                    LatestReplyAuthor = "@davidmiller",
                    LatestReplyTimeAgo = "1 hour ago",
                    CommentsCount = topic3Comments.Count,
                    TopCommenters = new List<UserViewModel> { users[3], users[1] },
                    Comments = topic3Comments
                },
                new TopicViewModel
                {
                    Id = 4,
                    Title = "UI of a new airline app, help needed!",
                    Text = "I'm working on a new airline booking app and need some feedback on the UI design. I've been struggling with the flight search results page layout...",
                    Category = "Feedback",
                    Author = users[7],
                    LatestReplyAuthor = "@janesmith",
                    LatestReplyTimeAgo = "2 hours ago",
                    CommentsCount = topic4Comments.Count,
                    TopCommenters = new List<UserViewModel> { users[4], users[5], users[2] },
                    Comments = topic4Comments
                }
            };

            List<TopicViewModel> viewModel = topics;
            return View(viewModel);
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
