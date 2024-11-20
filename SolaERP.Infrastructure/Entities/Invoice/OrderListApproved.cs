namespace SolaERP.Application.Entities.Invoice;

public class OrderListApproved : BaseEntity
{
    public int OrderMainId { get; set; }
    public string OrderNo { get; set; }
    public int OrderTypeId { get; set; }
	public int OrderComment { get; set; }
	public string Ordertype { get; set; }
    public string Currency { get; set; }
    public string VendorCode { get; set; }
    public int AgingDays { get; set; }
    public string VendorName { get; set; }
    public decimal TotalAmount { get; set; }
}