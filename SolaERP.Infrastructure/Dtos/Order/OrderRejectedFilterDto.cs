namespace SolaERP.Application.Dtos.Order;

public class OrderRejectedFilterDto
{
    public int BusinessUnitId { get; set; }
    public string[] ItemCode { get; set; }
    public int[] OrderTypeId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}