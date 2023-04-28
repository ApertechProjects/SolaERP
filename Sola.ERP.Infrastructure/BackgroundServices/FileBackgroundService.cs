using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SolaERP.Infrastructure.Contracts;
using SolaERP.Infrastructure.Models;
using System.Text;

namespace SolaERP.Infrastructure.BackgroundServices
{
    public class FileBackgroundService : BackgroundService
    {
        private readonly IOptions<Configurations.FileOptions> _option;
        private readonly IFileProcessor _processor;
        private readonly IConnection _conncection;
        private readonly IModel _channel;
        private readonly ILogger _logger;


        public FileBackgroundService(IOptions<Configurations.FileOptions> option, IFileProcessor processor, ConnectionFactory factory, ILogger logger)
        {
            _option = option;
            _processor = processor;
            _conncection = factory.CreateConnection();
            _channel = _conncection.CreateModel();
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"FileBackground service Started at:{DateTime.UtcNow}");

            stoppingToken.ThrowIfCancellationRequested();

            _channel.QueueDeclare(_option.Value.Queue,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false);


            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (sender, e) =>
            {
                try
                {
                    string fileModelJson = Encoding.UTF8.GetString(e.Body.ToArray());
                    FileModel file = JsonConvert.DeserializeObject<FileModel>(fileModelJson);

                    _logger.LogInformation($"File: {file.Filename} received.");
                    string filePath = await _processor.UploadAsync(file, file.GetPath(), stoppingToken);

                    _logger.LogInformation($"File: {file.Filename} proceded successfully.");
                    _channel.BasicAck(e.DeliveryTag, false);

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong while processing \n Error:{ex.Message}");
                }
            };

            _channel.BasicConsume(_option.Value.Queue, autoAck: false, consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
