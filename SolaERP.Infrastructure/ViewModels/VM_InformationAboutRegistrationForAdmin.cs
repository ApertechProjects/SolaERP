using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Language = SolaERP.Application.Enums.Language;

namespace SolaERP.Application.ViewModels
{
    public class VM_InformationAboutRegistrationForAdmin : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        public VM_InformationAboutRegistrationForAdmin()
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }
        public string UserName { get; set; }
        public string CompanyOrVendorName { get; set; }
        public string TemplateName => @"InformationAboutRegistrationForAdmin.cshtml";

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

    }
}
