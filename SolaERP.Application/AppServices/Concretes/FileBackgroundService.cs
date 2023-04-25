//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
//using RabbitMQ.Client;
//using SolaERP.Application.AppServices.Contract;
//using SolaERP.Infrastructure.Contracts.Repositories;

//namespace SolaERP.Application.AppServices.Concretes
//{
//    public class FileBackgroundService : BackgroundService
//    {
//        private readonly IFileService _fileService;
//        private readonly IUserRepository _userRepository;
//        private readonly IConnection _connection;
//        private readonly IModel _channel;
//        private readonly IOptions<Options.FileOptions> _fileOptions;


//        public FileBackgroundService(ConnectionFactory ConnectionFactory, IUserRepository userRepository, IFileService fileService,IOptions<Options.FileOptions>)
//        {
//            _userRepository = userRepository;
//            _fileService = fileService;

//            _connection = ConnectionFactory.CreateConnection();
//            _channel = _connection.CreateModel();
//        }


//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {


//        }
//    }
//}
