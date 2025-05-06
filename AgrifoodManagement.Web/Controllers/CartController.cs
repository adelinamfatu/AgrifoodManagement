using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models.Shop;
using AgrifoodManagement.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        public async Task<IActionResult> Checkout()
        {
            var deliveryMethod = "SameDay";

            decimal deliveryFee = deliveryMethod switch
            {
                "SameDay" => 10.00m,
                "Express" => 20.00m,
                "Normal" => 5.00m,
                _ => 5.00m
            };

            decimal discount = 0m;
            int discountPercentage = 0;

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
                DeliveryMethod = deliveryMethod,

                // 3. Payment
                PaymentMethod = "IPay",
                CardNumber = "•••• 5478",
                CardExpiryDate = "04/27",
                CardholderName = "John Doe",

                // 4. Order summary
                ItemCount = 3,
                Subtotal = 120.00m,
                Discount = discount,
                DiscountPercentage = discountPercentage,
                DeliveryFee = 5.00m,
                TotalAmount = 115.00m
            };

            ViewBag.CountryCodes = await GetCountryCodesAsync();

            return View("~/Views/Consumer/Checkout.cshtml", vm);
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

        [HttpPost]
        public IActionResult ApplyDiscount([FromBody] DiscountRequest dto)
        {
            var valid = dto.Code?.ToUpper() == "SAVE10";
            var pct = valid ? 10 : 0;
            var discount = dto.Subtotal * pct / 100m;
            var total = dto.Subtotal + dto.DeliveryFee - discount;

            return Json(new
            {
                Discount = discount,
                DiscountPercentage = pct,
                TotalAmount = total
            });
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateCheckoutSession([FromForm] CheckoutViewModel model)
        //{
        //    var domain = $"{Request.Scheme}://{Request.Host}";

        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = model.Items.Select(item => new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                UnitAmountDecimal = item.UnitPrice * 100, // in cents
        //                Currency = "eur",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.ProductName,
        //                },
        //            },
        //            Quantity = item.Quantity,
        //        }).ToList(),
        //        Mode = "payment",
        //        SuccessUrl = domain + Url.Action("Success", "Checkout"),
        //        CancelUrl = domain + Url.Action("Cancel", "Checkout"),
        //        CustomerEmail = model.Email
        //    };

        //    var service = new SessionService();
        //    Session session = await service.CreateAsync(options);

        //    Return the session ID to the client
        //    return Json(new { sessionId = session.Id, publishableKey = _stripeSettings.Value.PublishableKey });
        //}
    }
}
