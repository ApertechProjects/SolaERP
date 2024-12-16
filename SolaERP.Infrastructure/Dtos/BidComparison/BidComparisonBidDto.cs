namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonBidDto
    {
        public int LineNo { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public string RFQItem { get; set; }
        public string VendorName { get; set; }
        public string BidItem { get; set; }
        public int BidDetailId { get; set; }
        public int SelectedQTY { get; set; }
        public decimal BidQuantity { get; set; }
        public int BidComparisonBidId { get; set; }
        public decimal RfqDetailId { get; set; }
        public string PUOM { get; set; }
        public string CurrencyCode { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal ReportingAmount { get; set; }
        public string DeliverytermName { get; set; }
        public string DeliveryTime { get; set; }
        public string PaymentTermName { get; set; }
        public decimal Score { get; set; }

    }
}
