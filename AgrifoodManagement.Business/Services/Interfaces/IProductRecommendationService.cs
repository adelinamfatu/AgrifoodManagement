using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Interfaces
{
    public interface IProductRecommendationService
    {
        Task<List<Guid>> GetTopRecommendedAsync(
            string userEmail,
            int count,
            CancellationToken cancellationToken);
    }
}
