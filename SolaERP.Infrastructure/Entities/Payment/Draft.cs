namespace SolaERP.Application.Entities.Payment
{
    public class Draft : BaseEntity
    {
        public int PaymentDocumentMainId { get; set; }
        public string Priority { get; set; }
        public int PaymentDocumentTypeId { get; set; }
        public DateTime PaymentRequestDate { get; set; }
        public string PaymentRequestNo { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
    }
}
