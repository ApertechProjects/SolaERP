
using SolaERP.Application.Dtos.Attachment;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class VendorBankDetailDto
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string Beneficiary { get; set; }
        public string BeneficiaryTaxId { get; set; }
        public string Address { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string SWIFT { get; set; }
        public string BankCode { get; set; }
        public string Currency { get; set; }
        public string BankTaxId { get; set; }
        public string CorrespondentAccount { get; set; }
        public List<AttachmentDto> AccountVerificationLetter { get; set; }
    }
}
