using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.RFQ
{
    public class RequestRfqDto
    {
        public int Id { get; set; }
        public int DetailId { get; set; }   
        public long RowNum { get; set; }
        public string RequestNo { get; set; }
        public string RequestLine { get; set; }
        public decimal RequestQuantity { get; set; }
        public decimal OutStandingQTY { get; set; }
        public string ItemCode { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public int Condition { get; set; }
        public bool AlternativeItem { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string RUOM { get; set; }
        public string DefaultUOM { get; set; }
        public int CONV_ID { get; set; }
        public string Buyer { get; set; }
    }
}
