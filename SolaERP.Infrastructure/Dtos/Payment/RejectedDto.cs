namespace SolaERP.Application.Dtos.Payment
{
    public class RejectedDto
    {
        public int PaymentDocumentMainId { get; set; }
        public int PaymentDocumentPriorityId { get; set; }
        public int PaymentDocumentTypeId { get; set; }
        public DateTime PaymentRequestDate { get; set; }
        public string PaymentRequestNo { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string RejectComment { get; set; }
        public string Comment { get; set; }
    }
}
