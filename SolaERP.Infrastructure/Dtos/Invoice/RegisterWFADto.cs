namespace SolaERP.Application.Dtos.Invoice
{
    public class RegisterWFADto
    {
        public int InvoiceRegisterId { get; set; }
        public string ApproveStatus { get; set; }
        public string Status { get; set; }
        public int InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public int OrderType { get; set; }
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public string ReferenceDocNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string LineDescription { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public DateTime DueDate { get; set; }
        public int AgingDays { get; set; }
        public int MatchedAmount { get; set; }
        public int ProblematicInvoiceReasonId { get; set; }
        public string ReasonAdditionalDescription { get; set; }
        public int Sequence { get; set; }
    }
}
