using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Web.Models.Shop;
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

        public async Task<IActionResult> Checkout()
        {
            var vm = new CheckoutViewModel
            {
                // 1. Contact
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CountryCode = "+46",
                PhoneNumber = "0701234567",

                // 2. Delivery
                PostalCode = "12345",
                DeliveryMethod = "SameDay",

                // 3. Payment
                PaymentMethod = "IPay",
                CardNumber = "•••• 5478",
                CardExpiryDate = "04/27",
                CardholderName = "John Doe",

                // 4. Order summary
                ItemCount = 3,
                Subtotal = 120.00m,
                Discount = 10.00m,
                DiscountPercentage = 10,
                DeliveryFee = 5.00m,
                TotalAmount = 115.00m
            };

            return View("~/Views/Consumer/Checkout.cshtml", vm);
        }
    }
}
