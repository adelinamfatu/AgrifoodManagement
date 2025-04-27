using AgrifoodManagement.Business.Commands.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class CartController : Controller
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductToCartCommand command)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            command.BuyerEmail = email;
            var orderId = await _mediator.Send(command);

            return Ok(new { message = "Product added to cart", orderId });
        }

        [HttpDelete("Cart/RemoveItem/{orderDetailId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid orderDetailId)
        {
            try
            {
                await _mediator.Send(new RemoveCartItemCommand(orderDetailId));
                return Ok(new { message = "Item removed" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while removing the item." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartItemQuantityCommand command)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            command.BuyerEmail = email;
            await _mediator.Send(command);
            return Ok(new { message = "Quantity updated" });
        }
    }
}
