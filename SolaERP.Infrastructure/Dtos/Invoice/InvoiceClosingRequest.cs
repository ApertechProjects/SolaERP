namespace SolaERP.Application.Dtos.Invoice;

public class InvoiceClosingRequest
{
    public int BusinessUnitId { get; set; }
    public int AdvanceClosingId { get; set; }
    public int InvoiceregisterId { get; set; }
    public DateTime Date { get; set; }
    public decimal AllocatedAmount { get; set; }
    public string AccountCode { get; set; }
    public int AdvanceInvoicecRegisterId { get; set; }
    public decimal BaseAllocatedAmount { get; set; }
    public decimal ReportingAllocatedAmount { get; set; }
    public decimal VATAmount { get; set; }
    public decimal BaseVATAmount { get; set; }
    public decimal ReportingVATAmount { get; set; }
}