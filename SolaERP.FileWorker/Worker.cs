using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Shared;
using SolaERP.Application.UnitOfWork;
using System.Text;

namespace SolaERP.FileWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IOptions<QueueOption> _queueOption;
        private readonly IStorage _storage;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;


        public Worker(ILogger<Worker> logger, ConnectionFactory connectionFactory, IOptions<QueueOption> queueOption, IStorage storage, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _logger = logger;
            _queueOption = queueOption;
            _storage = storage;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("File Worker running at: {time}", DateTimeOffset.UtcNow);

                _channel.QueueDeclare(_queueOption.Value.Queue, true, false, false);

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.Received += async (sender, e) =>
                {
                    try
                    {
                        string fileModelJson = Encoding.UTF8.GetString(e.Body.ToArray());
                        FileModel model = JsonConvert.DeserializeObject<FileModel>(fileModelJson);

                        _logger.LogInformation($"File: {model?.fileName} received.");

                        string path = string.Empty;

                        if (model?.Type == Filetype.Profile)
                            path = await _storage.UploadAsync(model.FormDatas, model.Path, model.fileName, stoppingToken);

                        else
                            path = await _storage.UploadAsync(model?.FormDatas, model?.Path, model?.fileName, stoppingToken);

                        await _userRepository.UpdateImgesAsync(model?.Email, model.Type, path);
                        await _unitOfWork.SaveChangesAsync();

                        _logger.LogInformation($"File: {model?.fileName} proceded successfully.");

                        _channel.BasicAck(e.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("File processing failed ERR:" + ex.Message + "\n");
                    }
                };
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}