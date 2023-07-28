using SolaERP.Application.Enums;

namespace SolaERP.Application.Entities.RFQ
{
    public class RfqMain
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public int RFQType { get; set; }
        public string RFQNo { get; set; }
        public Emergency Emergency { get; set; }
        public DateTime RFQDate { get; set; }
        public DateTime RFQDeadline { get; set; }
        public string Buyer { get; set; }
        public Enums.Status Status { get; set; }
        public DateTime RequiredOnSiteDate { get; set; }
        public DateTime DesiredDeliveryDate { get; set; }
        public DateTime SentDate { get; set; }
        public int SingleUnitPrice { get; set; }
        public ProcurementType ProcurementType { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string Comment { get; set; }
        public string OtherReasons { get; set; }
        public int BusinessCategoryid { get; set; }
        public List<int> SingleSourceReasonId { get; set; }
        public int BiddingType { get; set; }
    }
}
