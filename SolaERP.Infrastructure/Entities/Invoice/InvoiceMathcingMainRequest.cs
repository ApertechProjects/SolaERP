namespace SolaERP.Application.Entities.Invoice;

public class InvoiceMathcingMain : BaseEntity
{
    public int InvoiceMatchingMainId { get; set; }
    public int BusinessUnitId { get; set; }
    public int OrderMainId { get; set; }
    public int InvoiceRegisterId { get; set; }
    public int WHT { get; set; }
    public int Comment { get; set; }
}