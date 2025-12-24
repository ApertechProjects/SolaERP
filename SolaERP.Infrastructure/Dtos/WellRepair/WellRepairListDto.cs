namespace SolaERP.Application.Dtos.WellRepair;

public class WellRepairListDto
{
    public string WellRepairId { get; set; }
    public string RepairCode { get; set; }
    public string RepairNameEng { get; set; }
    public string RepairNameAz { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}
