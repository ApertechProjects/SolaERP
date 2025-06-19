namespace SolaERP.Application.Dtos.Invoice
{
    public class RegisterListByOrderDto
    {
        public int InvoiceRegisterId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public string InvoiceComment { get; set; }
        public decimal OrderTotal { get; set; }
        public int? TaxId { get; set; }
        public int? InvoicePeriod { get; set; }
        // public string WithHoldingTaxCode { get; set; }
    }
}
