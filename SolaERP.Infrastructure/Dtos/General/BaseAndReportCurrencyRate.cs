namespace SolaERP.Application.Dtos.General;

public class BaseAndReportCurrencyRate
{
    public decimal BaseRate { get; set; }
    public short BaseMultiplyOrDivide { get; set; }
    public decimal ReportRate { get; set; }
    public short ReportMultiplyOrDivide { get; set; }
    public bool IsReportEqualsDisCount { get; set; }
}




public class BaseAndReportCurrencyRates
{
    public decimal BaseRate { get; set; }
    public string CurrCodeFrom { get; set; }
    public string CurrCodeTo { get; set; }
    public short BaseMultiplyOrDivide { get; set; }
    public decimal ReportRate { get; set; }
    public short ReportMultiplyOrDivide { get; set; }
    public bool IsReportEqualsDisCount { get; set; }
}