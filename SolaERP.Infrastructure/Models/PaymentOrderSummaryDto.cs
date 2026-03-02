namespace SolaERP.Application.Models;

public class PaymentOrderSummaryDto
{
    public decimal? OrderAmount { get; set; }
    public decimal? TotalPaid { get; set; }
    public string Currency { get; set; }
    public int PaymentPercent { get; set; }
}