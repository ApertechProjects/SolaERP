using System.Numerics;

namespace SolaERP.Application.Entities.Order;

public class OrderDetailLoadDto
{
    public int OrderDetailId { get; set; }
    public int OrderMainId { get; set; }
    public int LineNo { get; set; }
    public int? BidDetailId { get; set; }
    public int RequestDetailId { get; set; }
    public int? RFQRequestDetailId { get; set; }
    public DateTime OrderDate { get; set; }
    public string ItemCode { get; set; }
    public string ItemName1 { get; set; }
    public string ItemName2 { get; set; }
    public string UOM { get; set; }
    public string RUOM { get; set; }
    public string PUOM { get; set; }
    public int BusinessCategoryId { get; set; }
    public string BusinessCategoryCode { get; set; }
    public string BusinessCategoryName { get; set; }
    public decimal Quantity { get; set; }
    public decimal BidReqQTY { get; set; }
    public decimal OriginalQuantity { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public bool AlternativeItem { get; set; }
    public string AccountCode { get; set; }
    public string AccountName { get; set; }
    public int Condition { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal RemainingBudget { get; set; }
    public decimal LastUnitPrice { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public string LastVendorCode { get; set; }
    public decimal LastPriceManually { get; set; }
    public string OriginalItemCode { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal BaseRate { get; set; }
    public Int16 BaseMultplyDivide { get; set; }
    public decimal ReportingRate { get; set; }
    public Int16 ReportingMultplyDivide { get; set; }
    public decimal BaseTotal { get; set; }
    public decimal ReportingTotal { get; set; }
    public int DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal DiscountedAmount { get; set; }
    public decimal GrossAmount { get; set; }
    public int TaxId { get; set; }
    public string TaxCode { get; set; }
    public decimal TaxAmount { get; set; }
    public int Status { get; set; }
    public int ApproveStatus { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? RequestDeadline { get; set; }
    public string Buyer { get; set; }
    public decimal RequestQuantity { get; set; }
    public string RFQNo { get; set; }
    public decimal RFQQTY { get; set; }
    public string ComparisonNo { get; set; }
    public decimal BidQuantity { get; set; }
    public int OrderAnalysisId { get; set; }
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