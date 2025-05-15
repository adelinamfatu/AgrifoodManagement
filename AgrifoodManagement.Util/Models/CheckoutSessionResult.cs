using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class CheckoutSessionResult
    {
        public bool Success { get; init; }
        public string? SessionId { get; init; }
        public string? PublishableKey { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
