using SolaERP.Application.Entities.Currency;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class BankCodesDto
    {
        public List<Currency.CurrencyDto> Currencies { get; set; }
        public List<VendorBankDetail> BankDetails { get; set; }
    }
}
