using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Impl
{
    public class ProductRecommendationService : IProductRecommendationService
    {
        private readonly ApplicationDbContext _db;
        private const double MaxUrgencyDays = 30.0;

        public ProductRecommendationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Guid>> GetTopRecommendedAsync(
            string userEmail, int count, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // All products
            var products = await _db.Products
                .Where(p => p.AnnouncementStatus == AnnouncementStatus.Published
                         && p.ExpirationDate.Date >= today)
                .Select(p => new {
                    p.Id,
                    p.ExpirationDate,
                    p.IsPromoted,
                    p.UserId
                })
                .ToListAsync(cancellationToken);

            var productIds = products.Select(p => p.Id).ToList();

            // Wishlist
            var wishlist = new HashSet<Guid>();
            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = await _db.Users
                    .Where(u => u.Email == userEmail)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (user != default)
                {
                    wishlist = (await _db.WishlistItems
                        .Where(w => w.UserId == user && productIds.Contains(w.ProductId))
                        .Select(w => w.ProductId)
                        .ToListAsync(cancellationToken))
                        .ToHashSet();
                }
            }

            // Sentiment score per product
            var sentiment = await _db.Reviews
                .Where(r => productIds.Contains(r.ProductId) && r.SentimentType.HasValue)
                .GroupBy(r => r.ProductId)
                .Select(g => new {
                    ProductId = g.Key,
                    AvgSentiment = g.Sum(r => (
                                          r.SentimentType == SentimentType.Positive ? 1 :
                                          r.SentimentType == SentimentType.Negative ? -1 : 0
                                        ) * r.SentimentConfidence)
                                  / g.Sum(r => r.SentimentConfidence)
                })
                .ToDictionaryAsync(x => x.ProductId, x => x.AvgSentiment, cancellationToken);

            var scored = products
                .Select(p =>
                {
                    var days = (p.ExpirationDate.Date - today).TotalDays;
                    var urgency = 1.0 - Math.Min(Math.Max(days, 0), MaxUrgencyDays) / MaxUrgencyDays;

                    var wishBoost = wishlist.Contains(p.Id) ? 0.5 : 0.0;
                    sentiment.TryGetValue(p.Id, out var avgS);
                    var sentimentScore = (avgS + 1) / 2;

                    var baseScore = 0.5 * urgency
                              + 0.3 * sentimentScore
                              + 0.2 * wishBoost;

                    return new
                    {
                        p.Id,
                        p.IsPromoted,
                        Score = baseScore
                    };
                })
                .OrderByDescending(x => x.IsPromoted)
                .ThenByDescending(x => x.Score)
                .Select(x => x.Id)
                .Distinct()
                .Take(count)
                .ToList();

            return scored;
        }
    }
}
