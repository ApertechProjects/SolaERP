namespace SolaERP.Application.Entities.Order;

public class OrderAllDto : OrderTab
{
    public string Buyer { get; set; }
    public decimal Total { get; set; }
}