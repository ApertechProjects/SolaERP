using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels
{
    public class VM_ServiceOrderThreshold : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;

        public VM_ServiceOrderThreshold()
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }

        public string FullName { get; set; }
        public string Subject { get; set; }
        public string ServiceOrderNo { get; set; }
        public decimal Percentage { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Currency { get; set; }
        public string Url { get; set; }
        public EmailTemplateKey TemplateKey { get; set; }

        public string TemplateName()
        {
            return "ServiceOrderThreshold.cshtml";
        }

        public HtmlString? GenerateHeader()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Header?.ToString(), ServiceOrderNo)),
                Language.en => new HtmlString(string.Format(Header?.ToString(), ServiceOrderNo)),
                _ => new HtmlString(Header?.ToString())
            };
        }

        public HtmlString? GenerateSubject()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Subject?.ToString(), ServiceOrderNo)),
                Language.en => new HtmlString(string.Format(Subject?.ToString(), ServiceOrderNo)),
                _ => new HtmlString(Subject?.ToString())
            };
        }

        public HtmlString? GenerateBody()
        {
            var portalLink = !string.IsNullOrWhiteSpace(Url)
                ? @$"<b><a href=""{Url}"">Service Order Link</a></b>"
                : @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Client Portal</a></b>";

            return Language switch
            {
                Language.az => new HtmlString(string.Format(Body?.ToString(),
                    FullName, ServiceOrderNo, Percentage, TotalAmount, PaidAmount, Currency, portalLink)),
                Language.en => new HtmlString(string.Format(Body?.ToString(),
                    FullName, ServiceOrderNo, Percentage, TotalAmount, PaidAmount, Currency, portalLink)),
                _ => new HtmlString(Body?.ToString())
            };
        }
    }
}