namespace SolaERP.Application.Dtos.WellRepair;

public class WellRepairRequest
{
    public int? WellRepairId { get; set; }

    public string RepairCode { get; set; }

    public string RepairNameEng { get; set; }

    public string RepairNameAz { get; set; }

    public int? Status { get; set; }
    
}