namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonInfoDto : BaseEntity
    {
        public int BidComparisonId { get; set; }
        public string ComparisonNo { get; set; }
        public string Buyer { get; set; }
        public int BusinessUnitId { get; set; }
    }
}