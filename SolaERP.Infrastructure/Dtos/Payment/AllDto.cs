namespace SolaERP.Application.Dtos.Payment
{
    public class AllDto
    {
        public int PaymentDocumentMainId { get; set; }
        public string Priority { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaymentRequestDate { get; set; }
        public string PaymentRequestNo { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Status { get; set; }
        public string ApproveStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
        public DateTime? SentDate { get; set; }
        public long LineNo { get; set; }
    }
}
