using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Web.Mappers;
using AgrifoodManagement.Web.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize]
    public class ConsumerController : Controller
    {
        private readonly IMediator _mediator;

        public ConsumerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> HomeAsync()
        {
            var categories = await _mediator.Send(new GetCategoriesWithImagesQuery());
            var categoryViewModels = CategoryViewModelMapper.Map(categories);

            var dealViewModels = new List<ProductViewModel>
            {
                new ProductViewModel
                {
                    Name = "Organic Tomatoes",
                    Badge = "20% OFF",
                    ImageUrl = "/images/products/tomatoes.jpg",
                    CurrentPrice = 1.99m,
                    OriginalPrice = 2.49m,
                    AverageRating = 3.5m,
                    RatingCount = 120,
                },
                new ProductViewModel
                {
                    Name = "Fresh Carrots",
                    Badge = "30% OFF",
                    ImageUrl = "/images/products/carrots.jpg",
                    CurrentPrice = 0.99m,
                    OriginalPrice = 1.39m,
                    AverageRating = 4.84m,
                    RatingCount = 85,
                },
                new ProductViewModel
                {
                    Name = "Green Spinach Bunch",
                    Badge = "15% OFF",
                    ImageUrl = "/images/products/spinach.jpg",
                    CurrentPrice = 1.29m,
                    OriginalPrice = 1.49m,
                    AverageRating = 4.94m,
                    RatingCount = 62,
                },
                new ProductViewModel { Name = "Crisp Lettuce", Badge = "10% OFF", ImageUrl = "/images/products/lettuce.jpg", CurrentPrice = 1.10m, OriginalPrice = 1.22m, AverageRating = 5, RatingCount = 50 },
                new ProductViewModel { Name = "Sweet Corn", Badge = "25% OFF", ImageUrl = "/images/products/corn.jpg", CurrentPrice = 0.89m, OriginalPrice = 1.19m, AverageRating = 4.88m, RatingCount = 75 },
                new ProductViewModel { Name = "Baby Potatoes", Badge = "12% OFF", ImageUrl = "/images/products/potatoes.jpg", CurrentPrice = 1.39m, OriginalPrice = 1.59m, AverageRating = 3, RatingCount = 40 },
                new ProductViewModel { Name = "Bell Peppers", Badge = "18% OFF", ImageUrl = "/images/products/peppers.jpg", CurrentPrice = 1.59m, OriginalPrice = 1.89m, AverageRating = 5, RatingCount = 64 },
                new ProductViewModel { Name = "Zucchini", Badge = "22% OFF", ImageUrl = "/images/products/zucchini.jpg", CurrentPrice = 1.25m, OriginalPrice = 1.60m, AverageRating = 4, RatingCount = 37 },
                new ProductViewModel { Name = "Red Onions", Badge = "14% OFF", ImageUrl = "/images/products/red-onions.jpg", CurrentPrice = 1.05m, OriginalPrice = 1.22m, AverageRating = 4, RatingCount = 60 }
            };

            var viewModel = new HomeViewModel
            {
                Categories = categoryViewModels,
                Deals = dealViewModels
            };

            return View(viewModel);
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Locator()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Loyalty()
        {
            return View();
        }

        public IActionResult Support()
        {
            return View();
        }
    }
}
