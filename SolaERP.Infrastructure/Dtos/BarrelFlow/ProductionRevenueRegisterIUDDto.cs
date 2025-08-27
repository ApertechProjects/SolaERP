namespace SolaERP.Application.Dtos.BarrelFlow;

public class ProductionRevenueRegisterIUDDto
{
    public int ProductionRevenueRegisterId { get; set; }
    public int FactForecastId { get; set; }
    public string BatchNumber { get; set; }
    public int Status { get; set; }
    public int DeliveryQuarter { get; set; }
    public string DeliveryMonth { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal DeliveredGrossTon { get; set; }
    public decimal DeliveredNetTon { get; set; }
    public decimal BarrelFactor { get; set; }
    public int SalesQuarter { get; set; }
    public string SalesMonth { get; set; }
    public DateTime SalesDate { get; set; }
    public decimal SoldNetTon { get; set; }
    public decimal SalesPrice { get; set; }
    public decimal SoldBarrel { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal TransportBankFee { get; set; }
    public decimal MOBankfee { get; set; }
    public decimal CertifCustoms { get; set; }
    public decimal TranportRatePerTon { get; set; }
    public decimal AdvanceAmount { get; set; }
    public int AdvanceQuarter { get; set; }
    public string AdvanceMonth { get; set; }
    public DateTime AdvanceDate { get; set; }
    public int PaymentQuarter { get; set; }
    public string PaymentMonth { get; set; }
    public DateTime FinalPaymentDate { get; set; }
    public int UserId { get; set; }
    public int BusinessUnitId { get; set; }
}