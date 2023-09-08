namespace SolaERP.Application.Entities.Order;

public class OrderFilteredDto
{
    public int OrderMainId { get; set; }
    public int VendorId { get; set; }
    public string OrderNo { get; set; }
    public string OrderType { get; set; }
    public string VendorName { get; set; }
    public string Currency { get; set; }
    public string RFQNo { get; set; }
    public string BidNo { get; set; }
    public string ComparisonNo { get; set; }
    public string Comment { get; set; }
    public string EnteredBy { get; set; }
    public DateTime EnteredDate { get; set; }
    public string Status { get; set; }
    public string ApproveStatus { get; set; }
}