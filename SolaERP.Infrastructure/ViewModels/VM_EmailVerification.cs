using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_EmailVerification : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        public VM_EmailVerification()
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }

        public string? Token { get; set; }
        public string? Subject { get; set; }
        public string? Username { get; set; }
        public string GenerateButtonText()
        {
            return base.Language switch
            {
                Language.az => "E-poçtu indi təsdiq et",
                Language.en => "Verify Email Now",
            };
        }

        public string TemplateName()
        {
            return @"EmailVerification.cshtml";
        }

        public string ImageName()
        {
            return @"verification.png";
        }

        public string GetEmailVerifiedLink()
        {

            //return _configuration["Mail:ServerUrl"] + $"/EmailRedirectingPage/EmailVerified.html?verifyToken={Token}";
            var data = _configuration["Mail:Url"] + $"sources/templates/EmailConfirmPage.html?verifyToken={Token}";
            return _configuration["Mail:Url"] + $"sources/templates/EmailConfirmPage.html?verifyToken={Token}";
        }

    }
}
