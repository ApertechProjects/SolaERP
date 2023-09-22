namespace SolaERP.Application.Dtos.BidComparison;

public class HoldBidComparisonRequest
{
    public int BidComparisonId { get; set; }

    public int Sequence { get; set; }

    public int UserId { get; set; }

    public string Comment { get; set; }
}