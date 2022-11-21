using SolaERP.Application.Services;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Extensions
{
    /// <summary>
    /// This class is container for DataAcces Services 
    /// </summary>
    public static class DataAccessExtension
    {
        public static void UseSqlDataAccessServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IUnitOfWork, SqlUnitOfWork>();
            builder.Services.AddSingleton<UserService, UserService>();
            builder.Services.AddSingleton<IUserRepository, SqlUserRepository>();
            builder.Services.AddSingleton((t) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");
                return ConnectionFactory.CreateSqlConnection(connectionString);
            });
        }
    }
}
