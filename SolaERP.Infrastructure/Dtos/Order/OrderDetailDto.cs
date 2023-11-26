namespace SolaERP.Application.Dtos.Order;

public class OrderDetailDto
{
    private int? _requestDetailId { get; set; }
    public int OrderDetailid { get; set; }
    public int OrderMainId { get; set; }
    public int LineNo { get; set; }
    public int? BidDetailid { get; set; }
    public int? RequestDetailId { get; set; }
    public int? RFQRequestDetailid { get; set; }
    public DateTime OrderDate { get; set; }
    public string ItemCode { get; set; }
    public string RUOM { get; set; }
    public decimal Quantity { get; set; }
    public decimal OriginalQuantity { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public bool AlternativeItem { get; set; }
    public string AccountCode { get; set; }
    public int Condition { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal RemainingBudget { get; set; }
    public decimal? LastUnitPrice { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public string LastVendorCode { get; set; }
    public decimal? LastPriceManually { get; set; }
    public string OriginalItemCode { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public int DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal DiscountedAmount { get; set; }
    public decimal GrossAmount { get; set; }
    public int TaxId { get; set; }
    public decimal TaxAmount { get; set; }
    public int Status { get; set; }
    public int ApproveStatus { get; set; }
    public int CatId { get; set; }
    public string Requester { get; set; }
    public int AnalysisCode1Id { get; set; }
    public int AnalysisCode2Id { get; set; }
    public int AnalysisCode3Id { get; set; }
    public int AnalysisCode4Id { get; set; }
    public int AnalysisCode5Id { get; set; }
    public int AnalysisCode6Id { get; set; }
    public int AnalysisCode7Id { get; set; }
    public int AnalysisCode8Id { get; set; }
    public int AnalysisCode9Id { get; set; }
    public int AnalysisCode10Id { get; set; }
}