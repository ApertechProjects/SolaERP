using SolaERP.Application.Entities.Invoice;

namespace SolaERP.Application.Models
{
    public class InvoiceRegisterSaveModel
    {
        public int InvoiceRegisterId { get; set; }
        public int BusinessUnitId { get; set; }
        public int InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public int? OrderTypeId { get; set; }
        public int OrderMainId { get; set; }
        public string ReferenceDocNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string LineDescription { get; set; }
        public string VendorCode { get; set; }
        public int AgingDays { get; set; }
        public int? ProblematicInvoiceReasonId { get; set; }
        public string? AccountCode { get; set; }
        public int? WithHoldingTaxId { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? WithHoldingTaxAmount { get; set; }
        public decimal? GrossAmount { get; set; }
        public string? OrderReference { get; set; }
        public int? InvoicePeriod { get; set; }
		public string? VendorAccount { get; set; }
		public string? ReasonAdditionalDescription { get; set; }
		public string? Comment { get; set; }
		public int InvoiceTransactionTypeId { get; set; }
		public bool FullPrepaid { get; set; }
		public int? LinkedInvoiceRegisterId { get; set; }
		public bool UseOrderForInvoice { get; set; }
		
		public List<AttachmentSaveModel> Attachments { get; set; }
		public List<InvoiceRegisterDetails> Details { get; set; }
    }

  
}
