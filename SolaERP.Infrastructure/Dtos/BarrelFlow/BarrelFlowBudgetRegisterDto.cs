namespace SolaERP.Application.Dtos.BarrelFlow;

public class BarrelFlowBudgetRegisterDto
{
    public int BarrelFlowBudgetRegisterId { get; set; }
    public int Period { get; set; }
    public DateTime? Date { get; set; }
    public decimal ProductionGross { get; set; }
    public decimal LossPercent { get; set; }
    public decimal ProductionNet { get; set; }
    public decimal Delivery { get; set; }
    public decimal ExportOilPercent { get; set; }
    public decimal ExportOil { get; set; }
    public decimal COPercent { get; set; }
    public decimal CO { get; set; }
    public decimal BarrelFactor { get; set; }
    public int CreatedBy { get; set; }
    public string FullName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int BusinessUnitId { get; set; }
    public string Status { get; set; }
}