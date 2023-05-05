namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_EmailVerification
    {
        public string? Body { get; set; }
        public string? Token { get; set; }
        public string? Header { get; set; }
        public string? ButtonText { get; set; }
        public string? Footer { get; set; }
        public string? Subject { get; set; }
        public string? Username { get; set; }
        public string? CompanyName { get; set; }
        public int ActualYear { get => DateTime.UtcNow.Year; }
        public int Lang { get; set; }

        public string GetAllRightReservedText()
        {
            return null;
        }

        public string GenerateAutomaticheaderText() => null;
        public string GenerateFooterText() => null;
    }
}
