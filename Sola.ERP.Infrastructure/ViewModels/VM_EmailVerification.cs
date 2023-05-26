using Microsoft.CodeAnalysis.Diagnostics;
using SolaERP.Application.Enums;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_EmailVerification : VM_EmailTemplateBase
    {
        public string? Token { get; set; }
        public string? Footer { get; set; }
        public string? Subject { get; set; }
        public string? Username { get; set; }

        public string GenerateButtonText()
        {
            return base.Language switch
            {
                Language.Aze => "E-poçtu indi təsdiq et",
                Language.en => "Verify Email Now",
            };
        }
    }
}
