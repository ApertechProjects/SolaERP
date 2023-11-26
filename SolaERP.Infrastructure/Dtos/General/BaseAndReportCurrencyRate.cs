namespace SolaERP.Application.Dtos.General;

public class BaseAndReportCurrencyRate
{
    public decimal BaseRate { get; set; }
    public short BaseMultiplyOrDivide { get; set; }
    public decimal ReportRate { get; set; }
    public short ReportMultiplyOrDivide { get; set; }
    public bool IsReportEqualsDisCount { get; set; }
}