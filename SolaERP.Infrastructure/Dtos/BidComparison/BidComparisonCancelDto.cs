
namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonCancelDto
    {
        public List<int> BidComparisonIds { get; set; }
        public int CancelReasonId { get; set; }
        public string Comment { get; set; }
        public int BusinessUnitId { get; set; }
    }
}
