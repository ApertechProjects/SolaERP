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
    public decimal? UnrecoveredOpexBroughtEstimated { get; set; }
    public decimal? AOHEstimated { get; set; }
    public decimal? UnrecoveredCapexBroughtForwardEstimated { get; set; }
    public decimal? ReconciliationAmountFromQn2Estimated { get; set; }
    public decimal? OilNBVEstimated { get; set; }
    public decimal? TotalOpexEndEstimated { get; set; }
    public decimal? TotalCapexEndEstimated { get; set; }
    public decimal? RfactorEstimated { get; set; }
    public decimal? UnrecoveredOpexBroughtActual { get; set; }
    public decimal? AOHActual { get; set; }
    public decimal? UnrecoveredCapexBroughtForwardActual { get; set; }
    public decimal? AccumulatedReconciliationSOFAZActual { get; set; }
    public decimal? AccumulatedReconciliationSOAActual { get; set; }
    public decimal? CorrectionsPriorPeriodsOpexActual { get; set; }
    public decimal? CorrectionsPriorPeriodsCapexActual { get; set; }
    public decimal? AccumulatedReconciliationSOANBVActual { get; set; }
    public decimal? RfactorActual { get; set; }
}