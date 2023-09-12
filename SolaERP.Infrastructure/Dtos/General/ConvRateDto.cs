namespace SolaERP.Application.Dtos.General;

public class ConvRateDto
{
    public DateTime EffFromDateTime { get; set; }
    public DateTime EffToDateTime { get; set; }
    public string CurrCodeFrom { get; set; }
    public string CurrCodeTo { get; set; }
    public decimal ConvRate { get; set; }
    public short MultiplyDivide { get; set; }
}