using FluentValidation;
using SignalRChatExample.Hubs;
using SolaERP.Application.Services;
using SolaERP.Application.Validations;
using SolaERP.Application.Validations.UserValidation;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.DataAccess.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Extensions
{
    /// <summary>
    /// This class is container for DataAcces Services 
    /// </summary>
    public static class ServiceRegistrationsExtensions
    {
        public static void UseSqlDataAccessServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
            builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IGroupRepository, SqlGroupRepository>();

            builder.Services.AddScoped<IBusinessUnitService, BusinessUnitService>();
            builder.Services.AddScoped<IBusinessUnitRepository, SqlBusinessUnitRepository>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped((t) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");
                return ConnectionFactory.CreateSqlConnection(connectionString);
            });
        }

        public static void UseValidationExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidation>();
            builder.Services.AddScoped<ValidationFilter>();
        }

        public static void MapHubs(this WebApplication app)
        {
            app.MapHub<ChatHub>("/chatHub")
;
        }
    }
}
