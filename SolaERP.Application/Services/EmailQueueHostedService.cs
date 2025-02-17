using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SolaERP.Persistence.Services
{
    public class EmailQueueHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<EmailQueueHostedService> _logger;

        public EmailQueueHostedService(
            IBackgroundTaskQueue taskQueue,
            ILogger<EmailQueueHostedService> logger
        )
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
        }
    }
}