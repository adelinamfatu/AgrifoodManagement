using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using System.Text.Json;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class CartController : BaseUserController
    {
        private readonly HttpClient _httpClient;
        private readonly IStripeCheckoutService _stripeCheckoutService;

        public CartController(IMediator mediator, IStripeCheckoutService stripeCheckoutService, HttpClient httpClient)
            : base(mediator)
        {
            _httpClient = httpClient;
            _stripeCheckoutService = stripeCheckoutService;
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

        [HttpGet]
        public async Task<IActionResult> Checkout(string selectedDelivery = "Normal", string discountCode = "")
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var cartDto = await _mediator.Send(new GetCartByEmailQuery { BuyerEmail = email });

            var deliveryMethod = selectedDelivery ?? "Normal";
            var deliveryFee = DeliveryFeeMap.Options.TryGetValue(deliveryMethod, out var option)
                ? option.Fee
                : 0m;

            DiscountCodeDto? discountDto = null;
            if (!string.IsNullOrEmpty(discountCode))
            {
                discountDto = await _mediator.Send(new GetValidDiscountCodeQuery { Code = discountCode });

                if (discountDto == null)
                {
                    TempData["DiscountError"] = "That discount code isn’t valid.";
                }
            }

            var viewModel = CheckoutViewModelMapper.Map(cartDto, email, deliveryMethod, deliveryFee, discountDto);

            ViewBag.CountryCodes = await GetCountryCodesAsync();

            return View("~/Views/Consumer/Checkout.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionDto model)
        {
            var items = new List<StripeLineItemDto> {
                new StripeLineItemDto {
                    Name       = "Agrifood Order",
                    Description= $"Order {model.OrderId} via {model.DeliveryMethod}",
                    UnitAmount = (long)(model.TotalAmount * 100),
                    Currency   = "ron",
                    Quantity   = 1
                }
            };

            var domain = $"{Request.Scheme}://{Request.Host}";
            var success = $"{domain}/Cart/ConfirmOrderRedirect?session_id={{CHECKOUT_SESSION_ID}}";
            var cancel = $"{domain}/Cart/Checkout";

            var result = await _stripeCheckoutService.CreateCheckoutSessionAsync(
                lineItems: items,
                successUrl: success,
                cancelUrl: cancel,
                metadata: new Dictionary<string, string> { { "orderId", model.OrderId.ToString() } },
                paymentMethodTypes: new[] { "card" });

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Json(new
            {
                sessionId = result.SessionId,
                publishableKey = result.PublishableKey
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder([FromBody] ConfirmOrderCommand command)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var orderId = await _mediator.Send(command);
            return Ok(new { message = "Order saved", orderId });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmOrderRedirect(string session_id)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(session_id);

            if (session.PaymentStatus == "paid" && session.Metadata.TryGetValue("orderId", out var orderIdStr))
            {
                if (Guid.TryParse(orderIdStr, out var orderId))
                {
                    await _mediator.Send(new UpdateOrderStatusCommand(orderId, OrderStatus.Processing));
                }
            }

            return RedirectToAction("Home", "Consumer");
        }

        private async Task<List<object>> GetCountryCodesAsync()
        {
            var apiUrl = "https://restcountries.com/v2/all?fields=name,callingCodes";
            var fallbackPath = Path.Combine("C:\\AgrifoodManagement\\AgrifoodManagement.Util\\Data\\CountryCodes.json");

            try
            {
                var countries = await _httpClient.GetFromJsonAsync<List<RestCountry>>(apiUrl);

                return countries
                    .Where(c => c.CallingCodes != null && c.CallingCodes.Any())
                    .Select((c, index) => new
                    {
                        Text = $"+{c.CallingCodes[0]} – {c.Name}",
                        Value = index
                    })
                    .OrderBy(c => c.Value)
                    .Cast<object>()
                    .ToList();
            }
            catch (Exception ex)
            {
                if (!System.IO.File.Exists(fallbackPath))
                    return new List<object>(); 

                var json = await System.IO.File.ReadAllTextAsync(fallbackPath);
                var fallbackCodes = JsonSerializer.Deserialize<List<CountryCode>>(json);

                return fallbackCodes
                    .Select(c => new
                    {
                        c.Text,
                        c.Value
                    })
                    .Cast<object>()
                    .ToList();
            }
        }
    }
}
