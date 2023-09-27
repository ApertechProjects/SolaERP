using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;

namespace SolaERP.Application.Dtos.RFQ
{
    public class RFQInProgressDto
    {
        public int Id { get; set; }
        public int LineNo { get; set; }
        public DateTime RequiredOnSiteDate { get; set; }
        public Emergency Emergency { get; set; }
        public DateTime RFQDate { get; set; }
        public string RFQType { get; set; }
        public string RFQNo { get; set; }
        public DateTime DesiredDeliveryDate { get; set; }
        public string ProcurementType { get; set; }
        public string OtherReasons { get; set; }
        public DateTime SentDate { get; set; }
        public string Comment { get; set; }
        public DateTime RFQDeadline { get; set; }
        public string Buyer { get; set; }
        public bool SingleUnitPrice { get; set; }
        public string PlaceOfDelivery { get; set; }
        public int OfferCount { get; set; }
        public int Sent { get; set; }
        public int Accepted { get; set; }
        public int InProgress { get; set; }
        public int Responded { get; set; }
        public int Rejected { get; set; }
        public int NoResponse { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public bool HasAttachments { get; set; }
    }
}
