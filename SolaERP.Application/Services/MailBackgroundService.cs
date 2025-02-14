using Microsoft.Extensions.Hosting;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Persistence.Services
{
    public class MailBackgroundService : BackgroundService , IMailBackgroundService
    {
        private readonly IMailService _mailService;
        private readonly ITaskJob _taskJob;

        public MailBackgroundService(ITaskJob taskJob, IMailService mailService)
        {
            _taskJob = taskJob;
            _mailService = mailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_taskJob.Queue.Any())
                {
                    var msg = _taskJob.Queue.Dequeue();

                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        public async Task SendRFQVendorMail(int vendorId, String vendorName, int rfqId)
        {
            await Task.Delay(3000);
            _mailService.SendRFQVendorMail(vendorId, vendorName, rfqId);
        }
    }
}