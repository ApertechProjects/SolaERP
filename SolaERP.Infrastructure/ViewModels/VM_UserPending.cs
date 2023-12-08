using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Entities.General;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;
using Language = SolaERP.Application.Enums.Language;


namespace SolaERP.Application.ViewModels
{
    public class VM_UserPending : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        public VM_UserPending()
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }
        public string RequestNo { get; set; }
        public int? Sequence { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string ReasonDescription { get; set; }
        public EmailTemplateKey TemplateKey { get; set; }
        public string TemplateName()
        {
            switch (TemplateKey)
            {
                case EmailTemplateKey.REQP:
                    return "RequestPending.cshtml";
                case EmailTemplateKey.REQA:
                    return "RequestApproved.cshtml";
                case EmailTemplateKey.REQR:
                    return "RequestRejected.cshtml";
                case EmailTemplateKey.REQH:
                    return "RequestHeld.cshtml";
                default:
                    return "";
            }
        }

        public HtmlString? GenerateHeader()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Header?.ToString(), RequestNo, Sequence)),
                Language.en => new HtmlString(string.Format(Header?.ToString(), RequestNo, Sequence)),
            };
        }

        public HtmlString? GenerateSubject()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Subject?.ToString(), RequestNo, Sequence)),
                Language.en => new HtmlString(string.Format(Subject?.ToString(), RequestNo, Sequence)),
            };
        }

        public HtmlString? GenerateBody()
        {
            switch (TemplateKey)
            {
                case EmailTemplateKey.REQR:
                    return Language switch
                    {
                        Language.az => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo, @$"<b class = 'reason'> {ReasonDescription}</b>")),
                        Language.en => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo, @$"<b class = 'reason'> {ReasonDescription}</b>")),
                    };
                default:
                    return Language switch
                    {
                        Language.az => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo, @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Müştəri Portalına</a></b>")),
                        Language.en => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo, @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Client Portal</a></b>")),
                    };
            }
        }


    }
}
