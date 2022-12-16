using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SolaERP.Application.Identity_Server;
using SolaERP.Application.Services;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.DataAccess.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.UnitOfWork;
using SolaERP.Infrastructure.ValidationRules;
using SolaERP.Infrastructure.ValidationRules.UserValidation;

namespace SolaERP.Extensions
{
    /// <summary>
    /// This class is container for DataAcces Services 
    /// </summary>
    public static class ServiceRegistrations
    {
        private static void UseSqlConnection(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped((t) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");
                return ConnectionFactory.CreateSqlConnection(connectionString);
            });
        }
        private static void UseServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<IBusinessUnitService, BusinessUnitService>();
            builder.Services.AddScoped<IApproveStageMainService, ApproveStageMainService>();
            builder.Services.AddScoped<IApproveStageDetailService, ApproveStageDetailService>();
            builder.Services.AddScoped<IApproveStageRoleService, ApproveStageRoleService>();
            builder.Services.AddScoped<IApproveRoleService, ApproveRoleService>();
            builder.Services.AddScoped<IProcedureService, ProcedureService>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<IChatService, ChatService>();
        }
        private static void UseRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
            builder.Services.AddScoped<IGroupRepository, SqlGroupRepository>();
            builder.Services.AddScoped<IMenuRepository, SqlMenuRepository>();
            builder.Services.AddScoped<IBusinessUnitRepository, SqlBusinessUnitRepository>();
            builder.Services.AddScoped<IApproveStageMainRepository, SqlApproveStageMainRepository>();
            builder.Services.AddScoped<IApproveStageDetailRepository, SqlApproveStageDetailRepository>();
            builder.Services.AddScoped<IApproveStageRoleRepository, SqlApproveStageRoleRepository>();
            builder.Services.AddScoped<IApproveRoleRepository, SqlApproveRoleRepository>();
            builder.Services.AddScoped<IProcedureRepository, SqlProcedureRepository>();
            builder.Services.AddScoped<IChatRepository, SqlChatRepository>();
        }
        public static void UseValidationExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidation>();
            builder.Services.AddScoped<ValidationFilter>();
        }
        public static void UseIdentityService(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, Role>().AddDefaultTokenProviders();
            builder.Services.AddTransient<ITokenHandler, JwtTokenHandler>();
            builder.Services.AddScoped<IUserStore<User>, UserStore>();
            builder.Services.AddSingleton<IRoleStore<Role>, RoleStore>();
            builder.Services.AddSingleton<IPasswordHasher<User>, CustomPasswordHasher>();
        }
        public static void UseDataAccesServices(this WebApplicationBuilder builder)
        {
            builder.UseRepositories();
            builder.UseServices();
            builder.UseSqlConnection();
        }

    }
}
