namespace SolaERP.Application.Entities.Order;

public class OrderCreateRequestListDto
{
    public int RequestDetailId { get; set; }
    public int RequestMainId { get; set; }
    public string RequestNo { get; set; }
    public string RequestLineNo { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime RequestDeadline { get; set; }
    public DateTime RequestedDate { get; set; }
    public string ItemCode { get; set; }
    public string ItemName1 { get; set; }
    public string ItemName2 { get; set; }
    public string UOM { get; set; }
    public string Requester { get; set; }
    public decimal RequestQuantity { get; set; }
    public decimal RFQQuantity { get; set; }
    public decimal OrderQuantity { get; set; }
    public decimal RemainingQuantity { get; set; }
    public string RUOM { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Buyer { get; set; }
    public decimal AvailableQuantity { get; set; }
    public decimal QuantityFromStock { get; set; }
    public decimal OriginalQuantity { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal RemainingBudget { get; set; }
    public decimal Amount { get; set; }
    public string AccountCode { get; set; }
    public string AccountName { get; set; }
    public int Condition { get; set; }
    public int Priority { get; set; }
    public decimal ManualUP { get; set; }
    public bool AlternativeItem { get; set; }
    public int BusinessCategoryId { get; set; }
    public string BusinessCategoryName { get; set; }
    public string RequestType { get; set; }
    public int RequestTypeId { get; set; }
    public string Ordertype { get; set; }
    public int OrderTypeId { get; set; }
    public string TypeKey { get; set; }
    public string RequestComment { get; set; }
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
    public int CatId { get; set; }
    public int LineNo { get; set; }
}