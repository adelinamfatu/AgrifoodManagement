using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
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
            var viewModel = CategoryViewModelMapper.Map(categories);

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
