namespace SolaERP.Application.Models
{
    public class PaymentDocumentSaveModel
    {
        public PaymentDocumentMainSaveModel Main { get; set; }
        public List<PaymentDocumentDetailSaveModel> Details { get; set; }
    }

    public class PaymentDocumentSaveResultModel
    {
        public int PaymentDocumentMainId { get; set; }
        public string PaymentRequestNo { get; set; }
    }

    public class PaymentDocumentMainSaveModel
    {
        public int? PaymentDocumentMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public string VendorCode { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
        public int OrderAdvance { get; set; }
        public int PaymentAttachmentTypeId { get; set; }
        public int PaymentDocumentPriorityId { get; set; }
        public int ApproveStageMainId { get; set; }
        public string PaymentRequestNo { get; set; }
        public DateTime PaymentRequestDate { get; set; }
        public DateTime SentDate { get; set; }
    }

    public class PaymentDocumentDetailSaveModel
    {
        public int PaymentDocumentDetailId { get; set; }
        public int? PaymentDocumentMainId { get; set; }
        public string TransactionReference { get; set; }
        public string Reference { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public string AccountCode { get; set; }
        public string PaymentTerms { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PaymentRequestAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public decimal GRNAmount { get; set; }
        public DateTime DueDate { get; set; }

    }

}
