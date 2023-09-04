using SolaERP.Application.Dtos.General;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.Order;

public class OrderMainGetDto
{
    public List<DeliveryTerms> DeliveryTerms { get; set; }
    public List<PaymentTerms> PaymentTerms { get; set; }
    public List<Entities.SupplierEvaluation.Currency> Currencies { get; set; }
    public List<RejectReasonDto> RejectReasons { get; set; }
    public List<Country> Countries { get; set; }
}