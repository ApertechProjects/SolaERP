namespace SolaERP.Application.Dtos.BarrelFlow;

public class BarrelFlowUIDDto
{
    public int BarrelFlowRegisterId { get; set; }
    public int Period { get; set; }
    public DateTime Date { get; set; }
    public decimal OpeningOilStockTon { get; set; }
    public decimal BarrelFactor { get; set; }
    public decimal ProductionTon { get; set; }
    public decimal OtherUsage { get; set; }
    public decimal Delivery { get; set; }
    public decimal ExportOilPercent { get; set; }
    public decimal ExportOil { get; set; }
    public decimal COPercent { get; set; }
    public decimal CO { get; set; }
    public decimal ClosingOilStockTon { get; set; }
    public decimal CrudeOilTon { get; set; }
    public decimal ProcessOilTon { get; set; }
    public decimal PipelinesOilTon { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int BusinessUnitId { get; set; }
}