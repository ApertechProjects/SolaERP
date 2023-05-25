using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using RazorEngine;
using Language = SolaERP.Application.Enums.Language;
namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_RegistrationIsPendingAdminApprove : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        public VM_RegistrationIsPendingAdminApprove()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }
        public string UserName { get; set; }
        public string CompanyOrVendorName { get; set; }
        public string TemplateName()
        {
            return @"RegistrationIsPendingAdminApprove.cshtml";
        }

        public string ImageName()
        {
            return @"registrationPending.png";
        }

        public HtmlString? GenerateBody()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Body?.ToString(), @$"<a href={_configuration["Mail:ServerUrlUI"]}>Müştəri Portalına</a>")),
                Language.en => new HtmlString(string.Format(Body?.ToString(), @$"<a href={_configuration["Mail:ServerUrlUI"]}>Client Portal</a>")),
            };
        }

        public HtmlString GenerateUserInfo()
        {
            return Language switch
            {
                Language.en => new HtmlString($"Submitted User Name : {UserName}"),
                Language.az => new HtmlString($"Qeydiyyatdan keçən istifadəçi : {UserName}"),
            };
        }

        public HtmlString GenerateCompanyInfo()
        {
            return Language switch
            {
                Language.en => new HtmlString($"Company : {UserName}"),
                Language.az => new HtmlString($"Şirkət : {CompanyName}"),
            };
        }
    }
}
