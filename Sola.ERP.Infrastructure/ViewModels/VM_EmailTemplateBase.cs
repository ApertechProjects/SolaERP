using SolaERP.Application.Enums;


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
                Language.Eng => $@"© {ActualYear} Sola ERP All rights reserved.",
                Language.Ru => $@"© {ActualYear}  Sola ERP Все права защищены."
            };
        }

        public string GenerateAutomatedEmailText()
        {
            return this.Language switch
            {
                Language.Aze => "Bu avtomatik yaradılan e-poçtdur - lütfən cavab verməyin.",
                Language.Eng => "This is an automatically generated email - please do not reply",
                Language.Ru => "Это автоматически сгенерированное письмо - пожалуйста, не отвечайте"
            };
        }

        public string GenerateCompanyNameTeamText()
        {
            return this.Language switch
            {
                Language.Aze => $@"{CompanyName} Şirkəti.",
                Language.Eng => $@"{CompanyName} Team",
            };
        }
    }


}
