using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;
using Language = SolaERP.Application.Enums.Language;

namespace SolaERP.Application.ViewModels
{
    public class VM_RequestApprovalFlow : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        public VM_RequestApprovalFlow()
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

        public string? RequesterFullName { get; set; }
        public DateTime? AwaitingReviewSince { get; set; }
        public string? RequestUrl { get; set; }

        public string TemplateName()
        {
            switch (TemplateKey)
            {
                case EmailTemplateKey.R_R7:
                case EmailTemplateKey.R_W14:
                case EmailTemplateKey.R_F15:
                    return "RequestApprovalFlow.cshtml";
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
            var portalLink =
                !string.IsNullOrWhiteSpace(RequestUrl)
                    ? @$"<b><a href=""{RequestUrl}"">PR Link</a></b>"
                    : @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Client Portal</a></b>";

            var awaitingSinceText =
                AwaitingReviewSince.HasValue
                    ? AwaitingReviewSince.Value.ToString("yyyy-MM-dd")
                    : string.Empty;

            switch (TemplateKey)
            {
                case EmailTemplateKey.REQR:
                    return Language switch
                    {
                        Language.az => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo,
                            @$"<b class = 'reason'> {ReasonDescription}</b>")),
                        Language.en => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo,
                            @$"<b class = 'reason'> {ReasonDescription}</b>")),
                    };

                case EmailTemplateKey.R_R7:
                case EmailTemplateKey.R_W14:
                case EmailTemplateKey.R_F15:
                    // Placeholders for these templates:
                    // {0}=ApproverName, {1}=RequestNo, {2}=LinkHtml, {3}=RequesterFullName, {4}=AwaitingReviewSince
                    return Language switch
                    {
                        Language.az => new HtmlString(string.Format(Body?.ToString(),
                            FullName,
                            RequestNo,
                            portalLink,
                            RequesterFullName ?? string.Empty,
                            awaitingSinceText)),
                        Language.en => new HtmlString(string.Format(Body?.ToString(),
                            FullName,
                            RequestNo,
                            portalLink,
                            RequesterFullName ?? string.Empty,
                            awaitingSinceText)),
                    };

                default:
                    return Language switch
                    {
                        Language.az => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo,
                            @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Müştəri Portalına</a></b>")),
                        Language.en => new HtmlString(string.Format(Body?.ToString(), FullName, RequestNo,
                            @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Client Portal</a></b>")),
                    };
            }
        }
    }
}