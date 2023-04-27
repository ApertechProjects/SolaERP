using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SolaERP.Persistence.Services
{
    public class ImageSavingService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IModel _channel;
        private readonly IConnection _connection;

        private readonly string queueName;
        private readonly string exchange;
        private readonly string routing;

        public ImageSavingService(IConfiguration configuration)
        {
            _config = configuration;

            queueName = _config.GetSection("FileConsumer").GetChildren().FirstOrDefault(x => x.Key == "ListenQueue").Value;
            exchange = _config.GetSection("FileConsumer").GetChildren().FirstOrDefault(x => x.Key == "ExchangeName").Value;
            routing = _config.GetSection("FileConsumer").GetChildren().FirstOrDefault(x => x.Key == "RoutingKey").Value;


            _connection = new ConnectionFactory()
            {
                Uri = new(_config.GetSection("FileConsumer").GetChildren().FirstOrDefault(x => x.Key == "AMQP").Value)
            }.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();


            _channel.QueueDeclare(queueName, true, false, false);
            var consumer = new EventingBasicConsumer(_channel);

            Bind(queueName, exchange, routing);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    var messageBytes = e.Body;
                    await UploadAsync(messageBytes.ToArray());

                    _channel.BasicAck(e.DeliveryTag, false); ;
                }
                catch (Exception ex)
                {

                }

            };

            _channel.BasicConsume(queueName, false, consumer);
        }

        private void Bind(string queueName, string exchange, string routingKey)
        {
            _channel.QueueDeclare(queueName, true, false, false);
            _channel.QueueBind(queueName, exchange, routingKey);
        }

        private async Task UploadAsync(byte[] imageBytes)
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fileName = $"user-{Guid.NewGuid()}.png"; // Replace with desired file name and extension
            var imagePath = Path.Combine(wwwrootPath, fileName);

            await File.WriteAllBytesAsync(imagePath, imageBytes);
        }
    }
}
