namespace SolaERP.Application.Entities.BidComparison;

public class BidMainListByRfqMain : BaseEntity
{
    public string VendorName { get; set; }
    public string VendorCode { get; set; }
    public string BidNo { get; set; }
    public int BidMainId { get; set; }
    public decimal TotalDiscountedAmount { get; set; }
}