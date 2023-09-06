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
    public string DefaultUOM { get; set; }
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
    public int Condition { get; set; }
    public int Priority { get; set; }
    public decimal ManualUP { get; set; }
    public bool AlternativeItem { get; set; }
    public int BusinessCategoryId { get; set; }
    public string RequestType { get; set; }
    public int RequestTypeId { get; set; }
    public string Ordertype { get; set; }
    public string TypeKey { get; set; }
    public string RequestComment { get; set; }
}