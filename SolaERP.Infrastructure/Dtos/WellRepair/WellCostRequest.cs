namespace SolaERP.Application.Dtos.WellRepair;

public class WellCostRequest
{
    public int? WellCostId { get; set; }
    public int? BusinessUnitId { get; set; }
    public string? WellNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? NumberOfStomp { get; set; }
    public int? WellRepairId { get; set; }
    public string? Description { get; set; }
    public string? ProductionWay { get; set; }    
    public string? RepairNameAz { get; set; }
    public decimal? ProductionTon { get; set; }
    public string? Subject { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public int? ActualPeriod { get; set; }
}
