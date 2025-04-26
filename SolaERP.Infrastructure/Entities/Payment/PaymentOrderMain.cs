namespace SolaERP.Application.Entities.Payment
{
    public class PaymentOrderMain : BaseEntity
    {
        public int BusinessUnitId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public int PaymentOrderMainId { get; set; }
        public string PaymentOrderNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string BankAccount { get; set; }
        public decimal BankCharge { get; set; }
        public string BankChargeAccount { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int JournalNo { get; set; }
        public int AllocationReference { get; set; }
        public string CreatedBy { get; set; }
    }

    public class PaymentOrderDetail : BaseEntity
    {
        public long LineNo { get; set; }
        public int PaymentDocumentDetailId { get; set; }
        public int PaymentOrderDetailId { get; set; }
        public int PaymentOrderMainId { get; set; }
        public string PaymentRequestNo { get; set; }
        public DateTime PaymentRequestDate { get; set; }
        public string TransactionReference { get; set; }
        public string Reference { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PaymentRequestAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal GRNAmount { get; set; }
        public DateTime SentDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermCode { get; set; }
        public string PaymentTermName { get; set; }
        public int AgingDays { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string Department { get; set; }
        public string AnalysisCode1 { get; set; }
        public string AnalysisCode2 { get; set; }
        public string AnalysisCode3 { get; set; }
        public string AnalysisCode4 { get; set; }
        public string AnalysisCode5 { get; set; }
        public string AnalysisCode6 { get; set; }
        public string AnalysisCode7 { get; set; }
        public string AnalysisCode8 { get; set; }
        public string AnalysisCode9 { get; set; }
        public string AnalysisCode10 { get; set; }
    }
}