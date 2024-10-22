using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_VendorReject : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _lang;
        public VM_VendorReject(string lang)
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
                    "az" => "Vendor Ləğvi",
                    "en" => "Vendor Reject",
                    _ => "en"
                };
            }
        }
        public string? Username { get; set; }


        public string TemplateName()
        {
            return @"VendorReject.cshtml";
        }

        public string GetHeaderOfMail()
        {
            switch (_lang)
            {
                case "az":
                    return "Vendor ləğvi";
                case "en":
                    return "Vendor reject";
            }
            return "";
        }
        public HtmlString GetBodyOfMail()
        {
            switch (_lang)
            {
                case "en":
                    return new HtmlString("Dear Vendor, <br> " +
                        "Thank you for your interest in partnering with GL Group. <br>" +
                        "After careful review of your recent request to register in SOLA Procurement system, we regret to inform you that your registration request has not been successful at this time. <br>" +
                        "Should you have any questions or concerns regarding the rejection or the registration process, please feel free to contact us at sola@gl.world .<br>" +
                        "We appreciate your understanding and look forward to the opportunity to work with you in the future.");
                case "az":
                    return new HtmlString("Hörmətli Təchizatçı şirkət,<br> " +
                        "GL Group ilə əməkdaşlığ üçün marağınıza görə təşəkkür edirik. <br> " +
                        "SOLA Təchizat sistemində qeydiyyatdan keçmək üçün müraciətinizi diqqətlə nəzərdən keçirdikdən sonra təəssüflə bildiririk ki, qeydiyyat sorğunuz hazırda uğurlu olmayıb. <br>" +
                        "İmtina və ya qeydiyyat prosesi ilə bağlı hər hansı bir sualınız yaranarsa, sola@gl.world elektron poçt ünvanı ilə bizimlə əlaqə saxlaya bilərsiniz. <br>" +
                        "Anlayışınızı yüksək qiymətləndiririk və gələcəkdə sizinlə işləmək fürsətini gözləyirik.");
            }
            return new HtmlString("");
        }


    }
}
