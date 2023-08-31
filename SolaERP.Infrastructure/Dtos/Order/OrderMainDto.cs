namespace SolaERP.Application.Dtos.Order;

public class OrderMainDto
{
    public int? OrderMainId { get; set; }
    public int? BusinessUnitId { get; set; }
    public int? OrderTypeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public int? Emergency { get; set; }
    public string Buyer { get; set; }
    public int? ApproveStageMainId { get; set; }
    public string Comment { get; set; }
    public string VendorCode { get; set; }
    public string Currency { get; set; }
    public int? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public int? DeliveryTermId { get; set; }
    public string DeliveryTime { get; set; }
    public int? PaymentTermId { get; set; }
    public decimal? ExpectedCost { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? DesiredDeliveryDate { get; set; }
    public int? BidMainId { get; set; }
    public int? RFQMainId { get; set; }
    public int UserId { get; set; }
    public int NewOrderMainId { get; set; }
    public string NewOrderNo { get; set; }

    public List<OrderDetailDto> OrderDetails { get; set; }
}