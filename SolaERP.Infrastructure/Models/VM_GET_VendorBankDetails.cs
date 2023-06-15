using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class VM_GET_VendorBankDetails
    {
        public List<Currency> Currencies { get; set; }
        public List<VendorBankDetailDto> BankDetails { get; set; }
        public List<AttachmentDto> AccountVerificationLetter { get; set; }

    }

    public class BankAccountsDto
    {
        public VendorBankDetailDto BankDetails { get; set; }
        public AttachmentDto AccountVerificationLetter { get; set; }
    }
}
