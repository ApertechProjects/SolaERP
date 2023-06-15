using SolaERP.Application.Dtos.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class SupplierRegisterCommand
    {
        public CompanyInfoDto CompanyInformation { get; set; }
        public List<NonDisclosureAgreement> NonDisclosureAgreement { get; set; }
        public List<CodeOfBuConduct> CodeOfBuConduct { get; set; }
        public List<VendorBankDetailDto> BankAccounts { get; set; }
        public List<DueDiligenceChildDto> DueDiligence { get; set; }
        public List<PrequalificationDto> Prequalification { get; set; }
    }
}
