namespace SolaERP.Application.Dtos.RFQ
{
    public class RFQRequestDetailDto
    {
        public int Id { get; set; }
        public int RFQDetailId { get; set; }
        public int RequestDetailId { get; set; }
        public string RequestNo { get; set; }
        public string RequestLine { get; set; }
        public int RFQLine { get; set; }
        public string ItemCode { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public string DefaultUOM { get; set; }
        public int CONV_ID { get; set; }
        public Guid GUID { get; set; }
    }
}
