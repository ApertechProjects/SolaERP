namespace SolaERP.Application.Dtos.Entitlement;

public class EntitlementListDto
{
    public int? EntitlementRegisterId { get; set; }
    public int? Period { get; set; }
    public DateTime? Date { get; set; }
    public decimal? Opex { get; set; }
    public decimal? CorrectionToPriorPeriodsOpex { get; set; }
    public decimal? Capex { get; set; }
    public decimal? CorrectionToPriorPeriodsCapex { get; set; }
    public int? CreatedBy { get; set; }
    public string? CreatedName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? BusinessUnitId { get; set; }
}