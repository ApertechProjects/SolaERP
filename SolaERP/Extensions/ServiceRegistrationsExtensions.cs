using FluentValidation;
using SolaERP.Application.Services;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.DataAccess.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.UnitOfWork;
using SolaERP.Infrastructure.ValidationRules;
using SolaERP.Infrastructure.ValidationRules.UserValidation;

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
            builder.Services.AddScoped<IEmailService, EmailService>();
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
    }
}
