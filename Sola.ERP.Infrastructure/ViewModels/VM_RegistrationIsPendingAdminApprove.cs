using Microsoft.AspNetCore.Html;
using RazorEngine;
using Language = SolaERP.Application.Enums.Language;
namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_RegistrationIsPendingAdminApprove : VM_EmailTemplateBase
    {
        public string UserName { get; set; }
        public string CompanyOrVendorName { get; set; }
        public string TemplateName()
        {
            return @"RegistrationIsPendingAdminApprove.cshtml";
        }

        public string ImageName()
        {
            return @"registrationPending.png";
        }

        public HtmlString? GenerateBody()
        {
            return Language switch
            {
                Language.az => new HtmlString(string.Format(Body?.ToString(), @"<a href=""http://116.203.90.202:88/login"">Müştəri Portalına</a>")),
                Language.en => new HtmlString(string.Format(Body?.ToString(), @"<a href=""http://116.203.90.202:88/login"">Client Portal</a>")),
            };
        }

    }
}
