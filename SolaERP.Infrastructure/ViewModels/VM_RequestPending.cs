using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Infrastructure.ViewModels;
using Language = SolaERP.Application.Enums.Language;


namespace SolaERP.Application.ViewModels
{
    public class VM_RequestPending : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        public VM_RequestPending()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }
        public string RequestNo { get; set; }
        public int Sequence { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string TemplateName()
        {
            return "RequestPending.cshtml";
        }

        public HtmlString? GenerateBody()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Body?.ToString(), "hulya", RequestNo, @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Müştəri Portalına</a></b>")),
                Language.en => new HtmlString(string.Format(Body?.ToString(), "hulya", RequestNo, @$"<b><a href={_configuration["Mail:ServerUrlUI"]}>Client Portal</a></b>")),
            };
        }
    }
}
