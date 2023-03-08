using SolaERP.Business.Dtos.EntityDtos.Attachment;

namespace SolaERP.Business.Dtos.EntityDtos.Vendor
{
    public partial class VendorBank_Load
    {
        public System.Int32 BankId { get; set; }
        public System.Int32 VendorId { get; set; }
        public System.String BeneficiaryBankName { get; set; }
        public System.String CurrencyCode { get; set; }
        public System.String BankAccountNumber { get; set; }
        public System.String BeneficiaryBankAddress { get; set; }
        public System.String BeneficiaryBankAddress1 { get; set; }
        public System.String BeneficiaryFullName { get; set; }
        public System.String BeneficiaryAddress { get; set; }
        public System.String BeneficiaryAddress1 { get; set; }
        public System.String BeneficiaryBankCode { get; set; }
        public System.String IntermediaryBankCodeNumber { get; set; }
        public System.String IntermediaryBankCodeType { get; set; }
    }
    public partial class VendorBank_Load
    {
        public List<AttachmentList_Load> BNK { get; set; }
    }
}
