namespace SolaERP.Application.Dtos.RFQ
{
    public class RFQInProgressDto
    {
        public int RFQMainId { get; set; }
        public int LineNo { get; set; }
        public DateTime RequiredOnSiteDate { get; set; }
        public string Emergency { get; set; }
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
        public int BusinessCategoryId { get; set; }

        public int OfferCount { get; set; }
        public bool Sent { get; set; }
        public bool Accepted { get; set; }
        public bool InProgress { get; set; }
        public bool Responded { get; set; }
        public bool Rejected { get; set; }
        public bool NoResponse { get; set; }
        public string BusinessCategoryName { get; set; }
    }
}
