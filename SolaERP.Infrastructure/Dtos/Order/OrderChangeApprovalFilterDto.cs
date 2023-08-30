namespace SolaERP.Application.Dtos.Order;

public class OrderChangeApprovalFilterDto
{
    public int BusinessUnitId { get; set; }
    public string[] ItemCode { get; set; }
    public int[] OrderTypeId { get; set; }
}