using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SolaERP.Application;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Identity_Server;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Application.ValidationRules;
using SolaERP.Application.Validations.ApproveStageValidation;
using SolaERP.Application.Validations.AttachmentValidation;
using SolaERP.Application.Validations.GroupValidation;
using SolaERP.Application.Validations.RequestValidation;
using SolaERP.Application.Validations.UserValidation;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.DataAccess.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.Services.Storage;
using SolaERP.Persistence.Services;
using SolaERP.Persistence.Validations.AnalysisCodeValidation;
using SolaERP.Persistence.Validations.AnalysisStructure;
using SolaERP.Persistence.Validations.ApproveRoleValidation;
using SolaERP.Persistence.Validations.UserValidation;
using FileService = SolaERP.Infrastructure.Services.FileService;
using IFileService = SolaERP.Application.Contracts.Services.IFileService;
using UserValidation = SolaERP.Application.Validations.UserValidation.UserValidation;

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
            builder.Services.AddScoped<IApproveStageService, ApproveStageService>();
            builder.Services.AddScoped<IApproveStageDetailService, ApproveStageDetailService>();
            builder.Services.AddScoped<IApproveStageRoleService, ApproveStageRoleService>();
            builder.Services.AddScoped<IApproveRoleService, ApproveRoleService>();
            builder.Services.AddScoped<IProcedureService, ProcedureService>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<ILogInformationService, LogInformationService>();
            builder.Services.AddScoped<IAnalysisCodeService, AnalysisCodeService>();
            builder.Services.AddScoped<IStatusService, StatusService>();
            builder.Services.AddScoped<IBuyerService, BuyerService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IAccountCodeService, AccountCodeService>();
            builder.Services.AddScoped<ICurrencyCodeService, CurrencyCodeService>();
            builder.Services.AddScoped<IUOMService, UOMService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<ILayoutService, LayoutService>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
            builder.Services.AddScoped<IVendorService, VendorService>();
            //builder.Services.AddScoped<IFileService, FileService>();
            //builder.Services.AddScoped<IFileProducer, FileProducer>();
            builder.Services.AddScoped<IStorage, LocalStorage>();
            builder.Services.AddScoped<IAnalysisDimensionService, AnalysisDimensionService>();
            builder.Services.AddScoped<IAnalysisService, AnalysisService>();
            builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();

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
            builder.Services.AddScoped<IRequestMainRepository, SqlRequestMainRepository>();
            builder.Services.AddScoped<IRequestDetailRepository, SqlRequestDetailRepository>();
            builder.Services.AddScoped<ILogInformationRepository, SqlLogInformationRepository>();
            builder.Services.AddScoped<IItemCodeRepository, SqlItemCodeRepository>();
            builder.Services.AddScoped<IAnalysisStructureRepository, SqlAnalysisCodeRepository>();
            builder.Services.AddScoped<IStatusRepository, SqlStatusRepository>();
            builder.Services.AddScoped<IBuyerRepository, SqlBuyerRepository>();
            builder.Services.AddScoped<ILocationRepository, SqlLocationRepository>();
            builder.Services.AddScoped<IAccountCodeRepository, SqlAccountCodeRepository>();
            builder.Services.AddScoped<ICurrencyCodeRepository, SqlCurrencyCodeRepository>();
            builder.Services.AddScoped<IAttachmentRepository, SqlAttachmentRepository>();
            builder.Services.AddScoped<IUOMRepository, SqlUOMRepository>();
            builder.Services.AddScoped<ISupplierRepository, SqlSupplierRepository>();
            builder.Services.AddScoped<ILayoutRepository, SqlLayoutRepository>();
            builder.Services.AddScoped<ILanguageRepository, SqlLanguageRepository>();
            builder.Services.AddScoped<IVendorRepository, SqlVendorRepository>();
            builder.Services.AddScoped<IEmailNotificationRepository, SqlEmailNotificationRepository>();
            builder.Services.AddScoped<IAnalysisDimensionRepository, SqlAnalysisDimensionRepository>();
            builder.Services.AddScoped<INewAnalysisStructureRepository, SqlAnalysisStructureRepository>();
        }
        public static void UseValidationExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<AttachmentValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<RequestMainValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<ApprovalStageSaveValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<ApproveStageMainValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<ApproveStageDetailValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<ApproveStageRoleValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<ApprovalStatusValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<GroupBuyerSaveValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<UserSaveModelValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<ApproveRoleListSaveValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<ApproveRoleDeleteModel>();
            builder.Services.AddValidatorsFromAssemblyContaining<AnalysisCodeListSaveValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<AnalysisCodeDeleteValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<AnalysisStructureListSaveValidation>();
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
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.UseRepositories();
            builder.UseServices();
            builder.UseSqlConnection();
            builder.UseInfrastructureServices();
            builder.Services.AddMediatR();
        }

        private static void UseInfrastructureServices(this WebApplicationBuilder builder)
        {
            //builder.Services.AddScoped<IFileService, FileService>();
        }
    }
}
