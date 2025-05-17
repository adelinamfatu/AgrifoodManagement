using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Business.Queries.Shop;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Mappers;
using AgrifoodManagement.Web.Models.Locator;
using AgrifoodManagement.Web.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class ConsumerController : BaseUserController
    {
        public ConsumerController(IMediator mediator) : base(mediator) { }

        public async Task<IActionResult> HomeAsync()
        {
            var categories = await _mediator.Send(new GetCategoriesWithImagesQuery());
            var categoryViewModels = CategoryViewModelMapper.Map(categories);

            var dealsResult = await _mediator.Send(new GetTopDealsQuery(5));
            var dealViewModels = new List<ProductViewModel>();

            if (dealsResult.IsSuccess)
            {
                dealViewModels = ProductViewModelMapper.Map(dealsResult.Value);
            }

            var viewModel = new HomeViewModel
            {
                Categories = categoryViewModels,
                Deals = dealViewModels
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ShopAsync(int page = 1)
        {
            const int pageSize = 12;
            var result = await _mediator.Send(new GetProductsPerPageQuery(page, pageSize));

            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }

            var viewModel = new ShopViewModel
            {
                Products = ProductViewModelMapper.Map((List<ProductDto>)result.Value.Items),
                CurrentPage = result.Value.CurrentPage,
                TotalPages = result.Value.TotalPages
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = ProductViewModelMapper.MapOne(product);

            return View("ProductDetail", viewModel);
        }

        public async Task<IActionResult> BasketAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var cartDto = await _mediator.Send(new GetCartByEmailQuery { BuyerEmail = email });

            var viewModel = BasketViewModelMapper.Map(cartDto);

            ViewBag.BasketCount = viewModel.Items.Sum(x => x.QuantityOrdered);

            return View(viewModel);
        }

        public IActionResult Locator()
        {
            var jsonPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "scripts", "WorldMap.json");
            var mapJson = System.IO.File.ReadAllText(jsonPath);
            var mapData = JsonConvert.DeserializeObject(mapJson);

            var locations = new List<Location>
            {
                new() { latitude = 37.6276571, longitude = -122.4276688, name = "San Bruno" },
                new() { latitude = 33.5302186, longitude = -117.7418381, name = "Laguna Niguel" },
                new() { latitude = 40.7424509, longitude = -74.0081468, name = "New York" },
                new() { latitude = -23.5268201, longitude = -46.6489927, name = "Bom Retiro" },
                new() { latitude = 43.6533855, longitude = -79.3729994, name = "Toronto" },
                new() { latitude = 48.8773406, longitude = 2.3299627, name = "Paris" },
                new() { latitude = 52.4643089, longitude = 13.4107368, name = "Berlin" },
                new() { latitude = 19.1555762, longitude = 72.8849595, name = "Mumbai" },
                new() { latitude = 35.6628744, longitude = 139.7345469, name = "Minato" },
                new() { latitude = 51.5326602, longitude = -0.1262422, name = "London" }
            };

            var continentColors = new List<ContinentColor>
            {
                new() { continent = "North America", color = "#71b081" },
                new() { continent = "South America", color = "#5a9a77" },
                new() { continent = "Africa", color = "#498770" },
                new() { continent = "Europe", color = "#39776c" },
                new() { continent = "Asia", color = "#266665" },
                new() { continent = "Australia", color = "#124f5e" }
            };

            var viewModel = new LocatorViewModel
            {
                MapData = mapData,
                Locations = locations,
                ContinentColors = continentColors
            };

            return View(viewModel);
        }

        public async Task<IActionResult> HistoryAsync()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) 
                return RedirectToAction("Auth", "Account");

            var dto = await _mediator.Send(new GetProcessedOrdersQuery(email));
            var viewModel = OrderHistoryViewModelMapper.Map(dto);
            return View(viewModel);
        }

        public IActionResult Loyalty()
        {
            return View();
        }
    }
}
