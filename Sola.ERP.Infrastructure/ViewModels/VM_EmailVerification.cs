namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_EmailVerification : VM_EmailTemplateBase
    {
        public string? Token { get; set; }
        public string? ButtonText { get; set; }
        public string? Footer { get; set; }
        public string? Subject { get; set; }
        public string? Username { get; set; }
    }
}
