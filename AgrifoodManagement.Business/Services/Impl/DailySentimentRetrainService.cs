using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Impl
{
    public class DailySentimentRetrainService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DailySentimentRetrainService> _logger;
        private readonly TimeSpan _delay = TimeSpan.FromHours(24);

        public DailySentimentRetrainService(IServiceScopeFactory scopeFactory, ILogger<DailySentimentRetrainService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delayUntilMidnight = TimeUntilNextMidnight();
            await Task.Delay(delayUntilMidnight, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await RetrainModel(stoppingToken);
                await Task.Delay(_delay, stoppingToken);
            }
        }

        private async Task RetrainModel(CancellationToken ct)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var service = scope.ServiceProvider.GetRequiredService<ISentimentAnalysisService>();

                var allReviews = await db.Reviews
                                         .AsNoTracking()
                                         .ToListAsync(ct);

                var sw = Stopwatch.StartNew();
                await service.TrainAsync(allReviews, ct);
                sw.Stop();

                _logger.LogInformation("Sentiment model retrain completed in {Elapsed}s",
                                       sw.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sentiment retrain failed");
            }
        }

        static TimeSpan TimeUntilNextMidnight()
        {
            var now = DateTime.Now;
            var nextMidnight = now.Date.AddDays(1);
            return nextMidnight - now;
        }
    }
}
