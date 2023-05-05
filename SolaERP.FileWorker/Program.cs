using RabbitMQ.Client;
using Serilog;
using Serilog.Core;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Shared;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.FileWorker;
using SolaERP.Infrastructure.Services.Storage;

var progData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
Logger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logst.txt")
    .CreateLogger();



IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton((t) =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("DevelopmentConnectionString");
            return SolaERP.DataAccess.Factories.ConnectionFactory.CreateSqlConnection(connectionString);
        });
        services.AddHostedService<Worker>();
        services.Configure<QueueOption>(hostContext.Configuration.GetSection("FileOptions"));
        services.AddSingleton<IStorage, LocalStorage>();
        services.AddSingleton<IUserRepository, SqlUserRepository>();
        services.AddSingleton<IUnitOfWork, SqlUnitOfWork>();
        services.Configure<StorageOption>(hostContext.Configuration.GetSection("StorageServer"));


        _ = services.AddSingleton(serviceProvider => new ConnectionFactory()
        {
            Uri = new(hostContext?.Configuration["FileOptions:URI"]),
            DispatchConsumersAsync = true
        });
    })
    .Build();



await host.RunAsync();
