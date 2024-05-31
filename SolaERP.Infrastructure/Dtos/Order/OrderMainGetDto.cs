using SolaERP.Application.Dtos.Country;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.General;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Dtos.Order;

public class OrderMainGetDto
{
    public List<DeliveryTerms> DeliveryTerms { get; set; }
    public List<PaymentTerms> PaymentTerms { get; set; }
    public List<CurrencyDto> Currencies { get; set; }
    public List<RejectReasonDto> RejectReasons { get; set; }
    public List<CountryDto> Countries { get; set; }
    public List<TaxData> TaxDatas { get; set; }
}