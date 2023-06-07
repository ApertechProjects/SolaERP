using SolaERP.Application.Dtos.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class VM_RegisterSupplier
    {
        public PrequalificationDto Prequalification { get; set; }
        public List<NDADto> NonDisclosureAgreement { get; set; }
        public List<COBCDto> CodeOfBuConduct { get; set; }
        public List<VendorBankDetailDto> BankDetails { get; set; }
        public CompanyInfoDto CompanyInfo { get; set; }
    }
}
