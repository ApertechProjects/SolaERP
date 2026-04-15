namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonRFQDetails
    {
        public int Id { get; set; }
        public int? LineNumber { get; set; }
        public string Description { get; set; }
        public string UomName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Budget { get; set; }
        public decimal RemainingBudget { get; set; }
        public int? RfqDetailId { get; set; }
        public bool? AlternativeItem { get; set; }
        public string RfqDetailDescription { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string TechnicalQuality { get; set; }
    }
}
