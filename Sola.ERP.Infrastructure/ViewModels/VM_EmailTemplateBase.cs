using SolaERP.Application.Entities.Language;
using SolaERP.Application.Enums;
using System.Security.Cryptography.X509Certificates;
using Language = SolaERP.Application.Enums.Language;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_EmailTemplateBase
    {
        public string? Body { get; set; }
        public string? Header { get; set; }
        public string? CompanyName { get; set; }
        public Language Language { get; set; }

        private int ActualYear => DateTime.UtcNow.Year;

        public string GenerateCopyRightText()
        {
            return this.Language switch
            {
                Language.Aze => $@"© {ActualYear} Sola ERP bütün hüquqlar qorunur.",
                Language.en => $@"© {ActualYear} Sola ERP All rights reserved.",
                Language.Ru => $@"© {ActualYear}  Sola ERP Все права защищены."
            };
        }

        public string GenerateAutomatedEmailText()
        {
            return this.Language switch
            {
                Language.Aze => "Bu avtomatik yaradılan e-poçtdur - lütfən cavab verməyin.",
                Language.en => "This is an automatically generated email - please do not reply",
                Language.Ru => "Это автоматически сгенерированное письмо - пожалуйста, не отвечайте"
            };
        }

        public string GenerateCompanyNameTeamText()
        {
            return this.Language switch
            {
                Language.Aze => $@"{CompanyName} Şirkəti.",
                Language.en => $@"{CompanyName} Team",
            };
        }

        public string GenerateDevelopedInformationText()
        {
            return this.Language switch
            {
                Language.Aze => "Apertech Şirkəti tərəfindən hazırlanıb.",
                Language.en => "Developed by Apertech Team",
            };
        }
    }


}
