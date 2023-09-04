using Microsoft.AspNetCore.Html;
using Language = SolaERP.Application.Enums.Language;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_EmailTemplateBase
    {
        public HtmlString? Body { get; set; }
        public string? Header { get; set; }
        public string? CompanyName { get; set; }
        public Language Language { get; set; }

        private int ActualYear => DateTime.UtcNow.Year;

        public string GenerateCopyRightText()
        {
            return this.Language switch
            {
                Language.az => $@"© {ActualYear} Sola ERP bütün hüquqlar qorunur.",
                Language.en => $@"© {ActualYear} Sola ERP GetAll rights reserved.",
                //Language.ru => $@"© {ActualYear}  Sola ERP Все права защищены."
            };
        }

        public string GenerateAutomatedEmailText()
        {
            return this.Language switch
            {
                Language.az => "Bu avtomatik yaradılan e-poçtdur - lütfən cavab verməyin.",
                Language.en => "This is an automatically generated email - please do not reply",
                //Language.ru => "Это автоматически сгенерированное письмо - пожалуйста, не отвечайте"
            };
        }

        public string GenerateCompanyNameTeamText()
        {
            return this.Language switch
            {
                Language.az => $@"{CompanyName} Şirkəti.",
                Language.en => $@"{CompanyName} Team",
            };
        }

        public string GenerateDevelopedInformationText()
        {
            return this.Language switch
            {
                Language.az => "Apertech Şirkəti tərəfindən hazırlanıb.",
                Language.en => "Developed by Apertech Team",
            };
        }
    }


}
