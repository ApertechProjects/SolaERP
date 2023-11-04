using SolaERP.Application.Entities.Payment;

namespace SolaERP.Application.Dtos.Payment
{
    public class CreateOrderDto 
    {
        public string TransactionReference { get; set; }
        public string SystemInvoiceNo { get; set; }
        public string InvoiceNo { get; set; }
        public string Reference { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PaymentTerms { get; set; }
        public string CurrencyCode { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PaymentRequestAmount { get; set; }
        public decimal GRNAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string PaymentTermsName { get; set; }
        public int AgingDays { get; set; }
        public decimal PaidAmount { get; set; }
    }
}