namespace SolaERP.Application.Dtos.Buyer;

public class BuyerPurchaseOrderApproveEmailDto
{
    public int BusinessUnitId { get; set; }
    public string BusinessUnitCode { get; set; }
    public string BuyerCode { get; set; }
    public string BuyerName { get; set; }
    public string BuyerEmail { get; set; }
    public string OrderNo { get; set; }
    public int OrderId { get; set; }
    public string BusinessUnitName { get; set; }

}