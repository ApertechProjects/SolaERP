using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Shared;
using System.Text;

namespace SolaERP.Infrastructure.Services
{
    public class FileProducer : IFileProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IOptions<QueueOption> _options;

        public FileProducer(IOptions<QueueOption> options, ConnectionFactory facotry)
        {
            _options = options;
            _connection = facotry.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task ProduceAsync(IFormFile file, Filetype type, string email)
        {

            using MemoryStream ms = new();
            await file.CopyToAsync(ms);

            FileModel fileModel = new()
            {
                FormDatas = ms.ToArray(),
                fileName = Path.GetFileName(file.FileName),
                Type = type,
                Email = email
            };


            _channel.QueueDeclare(_options.Value.Queue, true, false, false);

            string fileModleJson = JsonConvert.SerializeObject(fileModel);
            byte[] jsonModelBytes = Encoding.UTF8.GetBytes(fileModleJson);

            _channel.BasicPublish(string.Empty, _options.Value.Queue, null, jsonModelBytes);
        }
    }
}
