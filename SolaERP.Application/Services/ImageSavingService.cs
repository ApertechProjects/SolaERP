using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace SolaERP.Application.Services
{
    public class ImageSavingService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public ImageSavingService(IConfiguration configuration)
        {
            _config = configuration;
        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
