namespace SolaERP.Application.Entities.Invoice
{
    public class InvoiceRegisterDetailDto
    {
        public int InvoiceMatchingDetailId { get; set; }
        public int InvoiceMatchingMainId { get; set; }
        public long LineNo { get; set; }
        public string OrderNo { get; set; }
        public object OrderLine { get; set; }
        public decimal Quantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public object MWP { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public object BaseRate { get; set; }
        public object ReportingRate { get; set; }
        public decimal BaseTotal { get; set; }
        public decimal ReportingTotal { get; set; }
    }
}
