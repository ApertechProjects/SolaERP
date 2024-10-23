using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_UserReject : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _lang;
        public VM_UserReject(string lang)
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
            _lang = lang;
        }

        public string? Token { get; set; }
        public string? Subject
        {
            get
            {
                return _lang switch
                {
                    "az" => "İstifadəçi Ləğvi",
                    "en" => "User Reject",
                    _ => "en"
                };
            }
        }
        public string? Username { get; set; }


        public string TemplateName()
        {
            return @"UserReject.cshtml";
        }

        public string GetHeaderOfMail()
        {
            switch (_lang)
            {
                case "az":
                    return "İstifadəçi ləğvi";
                case "en":
                    return "User reject";
            }
            return "";
        }
        public HtmlString GetBodyOfMail()
        {
            switch (_lang)
            {
                case "en":
                    return new HtmlString("Dear User, <br> " +
						"We regret to inform you that your confirmation has been rejected.");
                case "az":
                    return new HtmlString("Hörmətli Təchizatçı şirkət,<br> " +
						"Təəssüflə bildiririk ki, müraciətiniz rədd edilib.");
            }
            return new HtmlString("");
        }


    }
}
