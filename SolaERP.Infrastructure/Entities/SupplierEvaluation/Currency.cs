using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class Currency : BaseEntity
    {
        [DbColumn("CurrCode")]
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
    }
}
