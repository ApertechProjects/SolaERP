using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.ViewModels
{
    public class VM_UserApprove : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _lang;

        public VM_UserApprove(string lang)
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
                    "az" => "İstifadəçi təsdiqi",
                    "en" => "User Approve",
                    _ => "en"
                };
            }
        }
        public string? Username { get; set; }


        public string TemplateName()
        {
            return @"UserApprove.cshtml";
        }

        public string GetHeaderOfMail()
        {
            switch (_lang)
            {
                case "az":
                    return "İstifadəçi təsdiqi";
                case "en":
                    return "User approve";
            }
            return "";
        }
        public HtmlString GetBodyOfMail()
        {
            switch (_lang)
            {
                case "en":
                    return new HtmlString(String.Format("Dear {0}, <br> Congratulations! Your confirmation has been successfully completed.", Username));
                case "az":
                    return new HtmlString(String.Format("Hörmətli {0},<br> Təbrik edirik! Sizin təsdiqiniz uğurla tamamlandı.", Username));
            }
            return new HtmlString("");
        }


    }
}
