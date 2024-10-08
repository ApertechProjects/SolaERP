using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using System.Text;
using Language = SolaERP.Application.Enums.Language;
namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_RegistrationIsPendingAdminApprove : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        List<string> _changedFields;
        public VM_RegistrationIsPendingAdminApprove(List<string> changedFields = null)
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
            _changedFields = changedFields;
        }
        public string UserName { get; set; }
        public string CompanyOrVendorName { get; set; }
        public string TemplateName => @"RegistrationIsPendingAdminApprove.cshtml";

        public string ImageName => @"registrationPending.png";

        public HtmlString? GenerateBody()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Body?.ToString(), @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Müştəri Portalına</a></b>")),
                Language.en => new HtmlString(string.Format(Body?.ToString(), @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Client Portal</a></b>")),
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
                Language.en => new HtmlString($"Company : {CompanyName}"),
                Language.az => new HtmlString($"Şirkət : {CompanyName}"),
            };
        }

        public HtmlString GetDetailsAboutChanges()
        {
            if (_changedFields == null)
                return new HtmlString("");

            var language = Language switch
            {
                Language.en => new HtmlString($"Changed fields : "),
                Language.az => new HtmlString($"Dəyişdirilən dəyərlər : "),
            };
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(language);
            foreach (string field in _changedFields)
            {
                stringBuilder.Append(field + ",");
            }
            string result = stringBuilder.ToString().TrimEnd(',');
            return new HtmlString(result);
        }
    }
}
