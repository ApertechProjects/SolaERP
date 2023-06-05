using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class VM_GET_VendorBankDetails
    {
        public List<Currency> Currencies { get; set; }
        public List<VendorBankDetail> BankDetails { get; set; }

        //TODO: Implement Dto and mapping
    }
}
