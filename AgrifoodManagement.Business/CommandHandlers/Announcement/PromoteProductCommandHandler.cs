using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class PromoteProductCommandHandler : IRequestHandler<PromoteProductCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public PromoteProductCommandHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(PromoteProductCommand req, CancellationToken ct)
        {
            var product = await _context.Products.FindAsync(new object[] { req.ProductId }, ct);
            if (product == null) return false;

            // Mark the product as promoted
            product.IsPromoted = true;

            // Fetch the the paid amount
            var sessionSvc = new Stripe.Checkout.SessionService();
            var session = await sessionSvc.GetAsync(req.StripeIntentId, cancellationToken: ct);

            var amountDecimal = session.AmountTotal.GetValueOrDefault() / 100m;

            // Record the transaction in ExtendedProperties
            _context.ExtendedProperties.Add(new ExtendedProperty
            {
                ID = Guid.NewGuid(),
                EntityType = "ProductPromotion",
                EntityId = req.ProductId,
                Key = "StripeIntentId",
                Value = req.StripeIntentId
            });
            _context.ExtendedProperties.Add(new ExtendedProperty
            {
                ID = Guid.NewGuid(),
                EntityType = "ProductPromotion",
                EntityId = req.ProductId,
                Key = "PromotionFee",
                Value = amountDecimal.ToString("0.00")
            });

            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
