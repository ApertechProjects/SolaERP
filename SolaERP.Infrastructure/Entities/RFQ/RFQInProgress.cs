using SolaERP.Application.Attributes;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;

namespace SolaERP.Application.Entities.RFQ
{
    public class RFQInProgress : BaseEntity
    {
        [DbColumn("RFQMainId")]
        public int Id { get; set; }
        public long LineNo { get; set; }
        public string RFQType { get; set; }
        public string RFQNo { get; set; }
        public DateTime RFQDate { get; set; }
        public Emergency Emergency { get; set; }
        public int OfferCount { get; set; }
        public int Sent { get; set; }
        public int Accepted { get; set; }
        public int InProgress { get; set; }
        public int Responded { get; set; }
        public int Rejected { get; set; }
        public int NoResponse { get; set; }
        public DateTime DesiredDeliveryDate { get; set; }
        public string ProcurementType { get; set; }
        public string Buyer { get; set; }
        public DateTime RFQDeadline { get; set; }
        public DateTime SentDate { get; set; }
        public string Comment { get; set; }
        public int BusinessCategoryId { get; set; }
        public string BusinessCategoryName { get; set; }
        public bool HasAttachments { get; set; }
        public string BiddingType { get; set; }
    }
}
