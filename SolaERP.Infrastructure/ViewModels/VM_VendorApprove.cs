using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_VendorApprove : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _lang;
        public VM_VendorApprove(string lang)
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
            _lang = lang;
        }

        public string? Token { get; set; }
        public string? Subject { get; set; }
        public string? Username { get; set; }


        public string TemplateName()
        {
            return @"VendorApprove.cshtml";
        }

        public string GetHeaderOfMail()
        {
            switch (_lang)
            {
                case "az":
                    return "Vendor təsdiqi";
                case "en":
                    return "Vendor approve";
            }
            return "";
        }
        public HtmlString GetBodyOfMail()
        {
            switch (_lang)
            {
                case "az":
                    return new HtmlString("Hörmətli Təchizatçı şirkət, <br> " +
                        "Təqdim etdiyiniz məlumat/sənədlər üçün teşəkkür edirik.<br>" +
                        "Şirkətinizin müvafiq yoxlama prosesindən keçdiyini və GL Groupun təsdiq olunmuş təchizatçı siyahısına əlavə olunduğunu bildiririk.");
                case "en":
                    return new HtmlString("Dear Vendor,<br> " +
                        "Thank you very much for the provided information/documents.<br> " +
                        "We are happy to confirm that your company has passed the due diligence process and has been added to the GL vendor list. ");
            }
            return new HtmlString("");
        }


    }
}
