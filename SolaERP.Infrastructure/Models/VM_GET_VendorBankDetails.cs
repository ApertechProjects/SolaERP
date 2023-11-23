
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class VM_GET_VendorBankDetails
    {
        public List<Currency> Currencies { get; set; }
        public List<VendorBankDetailViewDto> BankDetails { get; set; }


    }

    public class BankAccountsDto
    {
        public VendorBankDetailDto BankDetails { get; set; }

    }
}
