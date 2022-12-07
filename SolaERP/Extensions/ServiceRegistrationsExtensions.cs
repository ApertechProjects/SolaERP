using FluentValidation;
using SolaERP.Application.Services;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.DataAccess.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
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
            #region UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
            #endregion
            #region User
            builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            #endregion
            #region Group
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IGroupRepository, SqlGroupRepository>();
            #endregion
            #region Menu
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<IMenuRepository, SqlMenuRepository>();
            #endregion
            #region BusinessUnit
            builder.Services.AddScoped<IBusinessUnitService, BusinessUnitService>();
            builder.Services.AddScoped<IBusinessUnitRepository, SqlBusinessUnitRepository>();
            #endregion
            #region ApproveStageMain
            builder.Services.AddScoped<IApproveStageMainService, ApproveStageMainService>();
            builder.Services.AddScoped<IApproveStageMainRepository, SqlApproveStageMainRepository>();
            #endregion
            #region ApproveStageDetail
            builder.Services.AddScoped<IApproveStageDetailService, ApproveStageDetailService>();
            builder.Services.AddScoped<IApproveStageDetailRepository, SqlApproveStageDetailRepository>();
            #endregion
            #region MailService
            builder.Services.AddScoped<IMailService, MailService>();
            #endregion
            #region ConnectionString
            builder.Services.AddScoped((t) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");
                return ConnectionFactory.CreateSqlConnection(connectionString);
            });
            #endregion
        }

        public static void UseValidationExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidation>();
            builder.Services.AddScoped<ValidationFilter>();
        }
    }
}
