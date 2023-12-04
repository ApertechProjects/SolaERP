using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Dtos.RFQ
{
    public class RFQMainDto
    {
        public int Id { get; set; }
        public BaseBusinessUnit BusinessUnit { get; set; }
        public RfqType RFQType { get; set; }
        public string RFQNo { get; set; }
        public Emergency Emergency { get; set; }
        public DateTime? RFQDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string EnteredBy { get; set; }
        public DateTime? RFQDeadline { get; set; }
        public string Buyer { get; set; }
        public Enums.Status Status { get; set; }
        public DateTime? RequiredOnSiteDate { get; set; }
        public DateTime? DesiredDeliveryDate { get; set; }
        public DateTime? SentDate { get; set; }
        public bool SingleUnitPrice { get; set; }
        public ProcurementType ProcurementType { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string Comment { get; set; }
        public string OtherReasons { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public List<SingleSourceReasonModel> SingleSourceReasons { get; set; }
        public BiddingType BiddingType { get; set; }
        public List<RFQDetailDto> Details { get; set; }
        public List<AttachmentDto> Attachments { get; set; }
    }
}