using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Util.ValueObjects;

namespace AgrifoodManagement.Business.Services.Interfaces
{
    public interface ISentimentAnalysisService
    {
        Task TrainAsync(IEnumerable<Review> allReviews, CancellationToken ct = default);
        (SentimentType Label, float Confidence) Predict(string comment);
    }
}
