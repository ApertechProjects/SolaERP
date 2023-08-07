using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.RFQ
{
    public class RfqAllDto : RfqBaseDto
    {
        public string Status { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
    }
}
