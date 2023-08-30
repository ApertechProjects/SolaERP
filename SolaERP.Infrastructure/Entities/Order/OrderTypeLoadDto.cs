namespace SolaERP.Application.Entities.Order;

public class OrderTypeLoadDto
{
    public int OrderTypeId { get; set; }
    public string OrderType { get; set; }
    public int ApproveStageMainId { get; set; }
}