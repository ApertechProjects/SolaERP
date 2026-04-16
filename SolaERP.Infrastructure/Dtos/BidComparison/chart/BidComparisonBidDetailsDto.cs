namespace SolaERP.Application.Dtos.BidComparison;

public class BidComparisonBidDetailsDto
{
    public int BidMainId { get; set; }
    public string BidNo { get; set; }
    public int RFQDetailId { get; set; }
    public decimal? Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountValue { get; set; }
    public bool AlternativeItem { get; set; }
    public string AlternativeDescription { get; set; }
    public int RfqLineNumber { get; set; }
    public bool IsBestLine { get; set; }
    public decimal PaymentTermScore { get; set; }
    public decimal DeliveryTermScore { get; set; }
    public decimal ScoreForBestLine { get; set; }
    public string ItemId { get; set; }
    public int IsUnderQuarter { get; set; }
    public int IsAboveQuarter { get; set; }
    public int ApproveStatusId { get; set; }
    public string PUOMName { get; set; }
    public bool HaveOffer { get; set; }
    public bool Selected { get; set; }
    
}