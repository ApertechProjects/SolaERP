namespace SolaERP.Application.Entities.Order;

public class OrderTypeLoadDto : BaseEntity
{
    public int OrderTypeId { get; set; }
    public string OrderType { get; set; }
    public string KeyCode { get; set; }
    public int ApproveStageMainId { get; set; }
}