namespace SolaERP.Application.Dtos.Order;

public class OrderIUDResponse
{
    public int OrderMainId { get; set; }
    public string OrderNo { get; set; }
    public List<int> OrderDetailIds { get; set; }
}