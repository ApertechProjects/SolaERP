using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;

namespace SolaERP.Application.Dtos.RFQ
{
    public class RFQDetailDto
    {
        public int Id { get; set; }
        public int RFQMainId { get; set; }
        public int LineNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public Condition Condition { get; set; } = Condition.Any;
        public bool AlternativeItem { get; set; }
        public string UOM { get; set; }
        public string DefaultUOM { get; set; }
        public decimal Quantity { get; set; }
        public int Conversion { get; set; }
        public string Description { get; set; }
        public Guid GUID { get; set; }
        public List<RFQRequestDetailDto> RequestDetails { get; set; }
    }
}
