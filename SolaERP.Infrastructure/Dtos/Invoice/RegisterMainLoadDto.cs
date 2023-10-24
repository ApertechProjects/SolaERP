namespace SolaERP.Application.Dtos.Invoice
{


    public class RegisterMainLoadDto
    {
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public string VendorCode { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceComment { get; set; }
        public string Comment { get; set; }
    }

    public class RegisterDetailLoadDto
    {
        public int InvoiceMatchingDetailId { get; set; }
        public int InvoiceMatchingMainId { get; set; }
        public long LineNo { get; set; }
        public string OrderNo { get; set; }
        public int OrderLine { get; set; }
        public decimal Quantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string MWP { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
