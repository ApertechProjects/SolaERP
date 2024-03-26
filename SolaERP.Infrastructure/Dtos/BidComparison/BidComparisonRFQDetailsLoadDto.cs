namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonRFQDetailsLoadDto
    {
        public int LineNo { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public decimal Budget { get; set; }
        public decimal RemainingBudget { get; set; }
        public int RFQDetailId { get; set; }
        public string Brand { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}