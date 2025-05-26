using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.History
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISentimentAnalysisService _sentiment;

        public AddReviewCommandHandler(ApplicationDbContext context, ISentimentAnalysisService sentimentAnalysisService)
        {
            _context = context;
            _sentiment = sentimentAnalysisService;
        }

        public async Task<bool> Handle(AddReviewCommand req, CancellationToken ct)
        {
            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == req.UserEmail, ct);

                if (user == null)
                {
                    return false;
                }

                var review = new Review
                {
                    Id = Guid.NewGuid(),
                    ProductId = req.ProductId,
                    Rating = req.Rating,
                    Comment = req.Comment,
                    ReviewerId = user.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var (label, confidence) = _sentiment.Predict(review.Comment);
                review.SentimentType = label;
                review.SentimentConfidence = confidence;

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync(ct);
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
