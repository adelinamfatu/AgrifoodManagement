using AgrifoodManagement.Util.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services
{
    public interface IStripeCheckoutService
    {
        Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
            IEnumerable<StripeLineItemDto> lineItems,
            string successUrl,
            string cancelUrl,
            Dictionary<string, string>? metadata = null,
            string mode = "payment",
            IEnumerable<string>? paymentMethodTypes = null);
    }
}
