namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonBidDetailsLoadDto
    {
        public int BidMainId { get; set; }
        public string BidNo { get; set; }
        public int RFQDetailId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountValue { get; set; }
        public bool AlternativeItem { get; set; }
        public string AlternativeDescription { get; set; }
        public decimal BaseTotalWithRate { get; set; }
        public decimal ConvertedGross { get; set; }
        public decimal Margins { get; set; }
    }
}