using AgrifoodManagement.Business.Queries.Account;
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

        public async Task<IActionResult> LocatorAsync()
        {
            var locations = await _mediator.Send(new GetSellerLocationsQuery());
            var viewModel = LocatorViewModelMapper.Map(locations);
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
