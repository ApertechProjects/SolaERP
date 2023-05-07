namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_RegistrationPending
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public int ActualYear { get => DateTime.UtcNow.Year; }
        public int Lang { get; set; }
        public string? Header { get; set; }
        public string? Body { get; set; }
        public string GenerateBodyText()
        {
            return Body
                   .Replace("@FullName", FullName)
                   .Replace("@UserName", UserName);
        }

        public string GetFooterText()
        {
            switch (Lang)
            {
                case 0:
                    CompanyName += " Şirkəti";
                    break;
                case 1:
                    CompanyName += " Team";
                    break;
            }
            return CompanyName;
        }
        public string GetAllRightReservedText()
        {
            string result = string.Empty;
            switch (Lang)
            {
                case 0:
                    result = @$"© {ActualYear} Sola ERP {CompanyName} Bütün hüquqlar qorunur.";
                    break;
                case 1:
                    result = $@"© {ActualYear} Sola ERP {CompanyName} All rights reserved";
                    break;
            }
            return result;
        }
    }
}
