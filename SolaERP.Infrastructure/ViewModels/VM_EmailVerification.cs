using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Enums;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_EmailVerification : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        public VM_EmailVerification()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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
            return _configuration["Url"] + $"/EmailRedirectingPage.html?verifyToken={Token}";
        }

    }
}
