using SolaERP.Application.Dtos.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class SupplierRegisterCommand
    {
        public CompanyInfoDto CompanyInformation { get; set; }
        public List<NonDisclosureAgreement> NonDisclosureAgreement { get; set; }
        public List<CodeOfBuConduct> CodeOfBuConduct { get; set; }
        public List<VendorBankDetailDto> BankAccounts { get; set; }
        public List<DueDiligenceDesignSaveDto> DueDiligence { get; set; }
        public List<PrequalificationDesignListDto> Prequalification { get; set; }
    }


    public class SupplierRegisterCommand2
    {
        public CompanyInfoDto CompanyInformation { get; set; }
        public List<NonDisclosureAgreement> NonDisclosureAgreement { get; set; }
        public List<CodeOfBuConduct> CodeOfBuConduct { get; set; }
        public List<VendorBankDetailDto> BankAccounts { get; set; }
        public List<DueDiligenceDesignSaveDto> DueDiligence { get; set; }
        public List<PrequalificationDesignListDto> Prequalification { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsRevise { get; set; }
    }
}