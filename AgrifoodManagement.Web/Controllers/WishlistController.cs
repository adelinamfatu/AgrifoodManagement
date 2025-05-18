using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Mappers;
using AgrifoodManagement.Web.Models.Shop;
using AgrifoodManagement.Web.Models.Wishlist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class WishlistController : BaseUserController
    {
        public WishlistController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var itemsDto = await _mediator.Send(new GetUserWishlistQuery(email));
            var viewModel = WishlistViewModelMapper.Map(itemsDto);
            return View("~/Views/Consumer/Wishlist.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle([FromBody] Guid productId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var isFavorited = await _mediator
                .Send(new ToggleWishlistItemCommand(userEmail, productId));

            return Ok(new { isFavorited });
        }
    }
}
