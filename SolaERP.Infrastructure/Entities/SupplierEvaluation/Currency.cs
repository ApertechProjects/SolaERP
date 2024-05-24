using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class Currency : BaseEntity
    {
        public string CurrCode { get; set; }
        public string Description { get; set; }
    }
}
