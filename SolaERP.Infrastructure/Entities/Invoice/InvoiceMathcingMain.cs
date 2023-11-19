namespace SolaERP.Application.Entities.Invoice;

public class InvoiceMathcingMain : BaseEntity
{
    public int InvoiceMatchingMainId { get; set; }
    public int BusinessUnitId { get; set; }
    public int OrderMainId { get; set; }
    public int InvoiceRegisterId { get; set; }
    public decimal WHT { get; set; }
    public string Comment { get; set; }
}