using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Entities.RFQ
{
    public class RFQBase
    {
        public int RFQMainId { get; set; }
        public long LineNo { get; set; }
        public DateTime? RequiredOnSiteDate { get; set; }
        public string Emergency { get; set; }
        public DateTime? RFQDate { get; set; }
        public string RFQType { get; set; }
        public string RFQNo { get; set; }
        public DateTime? DesiredDeliveryDate { get; set; }
        public string ProcurementType { get; set; }
        public string OtherReasons { get; set; }
        public DateTime? SentDate { get; set; }
        public string Comment { get; set; }
        public DateTime? RFQDeadline { get; set; }
        public string Buyer { get; set; }
        public bool SingleUnitPrice { get; set; }
        public string PlaceOfDelivery { get; set; }
        public int BusinessCategoryId { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public int BiddingType { get; set; }
        public bool HasAttachments { get; set; }
        public string EnteredBy { get; set; }
    }
}