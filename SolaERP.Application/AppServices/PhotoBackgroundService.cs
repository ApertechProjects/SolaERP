using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace SolaERP.Application.AppServices
{
    public class PhotoBackgroundService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private IConfiguration _config;
        private readonly IHostingEnvironment hostingEnvironment;

        public PhotoBackgroundService(ConnectionFactory connectionFactory, IConfiguration config)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _config = config;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        private void SaveUserImage(byte[] staticFile)
        {



        }

    }
}
