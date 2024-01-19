namespace SolaERP.Application.Dtos.Invoice
{
    public class MatchingMainServiceDto
    {
        public int OrderMainId { get; set; }
        public long LineNo { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderTotal { get; set; }
        public string OrderType { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal MatchedAmount { get; set; }
        public decimal UnmatchedAmount { get; set; }
        public string MatchStatus { get; set; }
    }
}
