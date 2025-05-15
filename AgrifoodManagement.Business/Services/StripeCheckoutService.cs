using AgrifoodManagement.Util.Models;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace AgrifoodManagement.Business.Services
{
    public class StripeCheckoutService : IStripeCheckoutService
    {
        private readonly StripeSettings _settings;

        public StripeCheckoutService(IOptions<StripeSettings> opts)
        {
            _settings = opts.Value;
            StripeConfiguration.ApiKey = _settings.SecretKey;
        }

        public async Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
            IEnumerable<StripeLineItemDto> lineItems,
            string successUrl,
            string cancelUrl,
            Dictionary<string, string>? metadata = null,
            string mode = "payment",
            IEnumerable<string>? paymentMethodTypes = null)
        {
            try
            {
                var methods = paymentMethodTypes?.ToList()
                              ?? new List<string> { "card" };

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = methods,
                    LineItems = lineItems.Select(item => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = item.UnitAmount,
                            Currency = item.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name,
                                Description = item.Description
                            }
                        },
                        Quantity = item.Quantity
                    }).ToList(),
                    Mode = mode,
                    Metadata = metadata,
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return new CheckoutSessionResult
                {
                    Success = true,
                    SessionId = session.Id,
                    PublishableKey = _settings.PublishableKey
                };
            }
            catch (Exception ex)
            {
                return new CheckoutSessionResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
