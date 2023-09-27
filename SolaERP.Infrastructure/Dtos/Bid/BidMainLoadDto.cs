using SolaERP.Application.Dtos.RFQ;
using AttachmentDto = SolaERP.Application.Dtos.Attachment.AttachmentDto;

namespace SolaERP.Application.Dtos.Bid
{
    public class BidMainLoadDto
    {
        public int BidMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public int RFQMainId { get; set; }
        public string EnteredBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string BidNo { get; set; }
        public string ComparisonNo { get; set; }
        public string OrderNo { get; set; }
        public string OperatorComment { get; set; }
        public string VendorCode { get; set; }
        public string CurrencyCode { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountValues { get; set; }
        public string DeliveryTerms { get; set; }
        public string DeliveryTime { get; set; }
        public string PaymentTerms { get; set; }
        public decimal ExpectedCost { get; set; }
        public int Status { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApproveStageMainId { get; set; }
        public bool Discualified { get; set; }
        public string DiscualificationReason { get; set; }

        public RFQMainDto RFQMain { get; set; }
        public List<BidDetailsLoadDto> Details { get; set; }
        public List<AttachmentDto> Attachments { get; set; }
    }
}
