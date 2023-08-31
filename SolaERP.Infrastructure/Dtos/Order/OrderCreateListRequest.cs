namespace SolaERP.Application.Dtos.Order;

public class OrderCreateListRequest
{
    public int BusinessUnitId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}