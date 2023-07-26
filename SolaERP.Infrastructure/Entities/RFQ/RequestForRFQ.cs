namespace SolaERP.Application.Entities.RFQ
{
    public class RequestForRFQ : BaseEntity
    {
        public int RequestMainId { get; set; }
        public long RowNum { get; set; }
        public string RequestLine { get; set; }
        public decimal Quantity { get; set; }
        public int OutStandingQTY { get; set; }
        public decimal OriginalQuantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string UOM { get; set; }
        public string DefaultUOM { get; set; }
        public int CONV_ID { get; set; }
        public string Buyer { get; set; }
    }
}
