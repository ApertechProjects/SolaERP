using SolaERP.Application.Dtos.Attachment;
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
        public string BusinessCategoryName { get; set; }
        public int BusinessCategoryId { get; set; }
        public int Condition { get; set; } = 1;
        public bool AlternativeItem { get; set; }
        public string UOM { get; set; }
        public string DefaultUOM { get; set; }
        public decimal Quantity { get; set; }
        public decimal Conversion { get; set; }
        public decimal ConversionRate { get; set; }
        public string Description { get; set; }
        public Guid GUID { get; set; }
        public List<RFQRequestDetailDto> RequestDetails { get; set; }
        public List<AttachmentDto> Attachments { get; set; }
    }
}
