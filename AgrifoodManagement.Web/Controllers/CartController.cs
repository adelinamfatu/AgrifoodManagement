using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Mappers;
using AgrifoodManagement.Web.Models.Shop;
using AgrifoodManagement.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using System.Text.Json;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class CartController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOptions<StripeSettings> _stripeSettings;
        private readonly HttpClient _httpClient;

        public CartController(IMediator mediator, IOptions<StripeSettings> stripeSettings, HttpClient httpClient)
        {
            _mediator = mediator;
            _stripeSettings = stripeSettings;
            _httpClient = httpClient;
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
            }

            var viewModel = CheckoutViewModelMapper.Map(cartDto, email, deliveryMethod, deliveryFee, discountDto);

            ViewBag.CountryCodes = await GetCountryCodesAsync();

            return View("~/Views/Consumer/Checkout.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionDto model)
        {
            StripeConfiguration.ApiKey = _stripeSettings.Value.SecretKey;

            if (!ModelState.IsValid)
                return BadRequest("Invalid checkout data");

            var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = (long)(model.TotalAmount * 100),
                            Currency = "ron",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Agrifood Order",
                                Description = $"Delivery: {model.DeliveryMethod}"
                            },
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = domain + "/Cart/Success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/Cart/Cancel"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Json(new
            {
                sessionId = session.Id,
                publishableKey = _stripeSettings.Value.PublishableKey
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

        private async Task<(decimal? lat, decimal? lng)> GeocodeAddressAsync(string postalCode, string countryCode, string? city = null, string? street = null)
        {
            var address = $"{street} {city} {postalCode} {countryCode}".Trim();
            var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "AgroFoodManagementApp");

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return (null, null);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<NominatimResult>>(content);

            if (result?.Any() == true)
            {
                var location = result.First();
                return (decimal.Parse(location.lat), decimal.Parse(location.lon));
            }

            return (null, null);
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
