using FluentValidation;
using SolaERP.Application.Services;
using SolaERP.Business.Dtos.EntityDtos.BU;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.DataAccess.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.UnitOfWork;
using SolaERP.Infrastructure.ValidationRules;
using SolaERP.Infrastructure.ValidationRules.UserValidation;

namespace SolaERP.Extensions
{
    /// <summary>
    /// This class is container for DataAcces Services 
    /// </summary>
    public static class DataAccessExtension
    {
        public static void UseSqlDataAccessServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
            builder.Services.AddScoped<UserService, UserService>();
            builder.Services.AddScoped<IUserRepository, SqlUserRepository>();

            builder.Services.AddScoped<BusinessUnitService, BusinessUnitService>();
            builder.Services.AddScoped<IBusinessUnitRepository, SqlBusinessUnitRepository>();
            builder.Services.AddScoped((t) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");
                return ConnectionFactory.CreateSqlConnection(connectionString);
            });
        }

        public static void ValidationExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidation>();
            builder.Services.AddScoped<ValidationFilter>();
        }
    }
}
