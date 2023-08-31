namespace SolaERP.Application.Dtos.Order;

public class OrderFilterDto
{
    public int BusinessUnitId { get; set; }
    public string[] ItemCode { get; set; }
    public int[] OrderTypeId { get; set; }
    public int[] Status { get; set; }
    public int[] ApproveStatus { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}