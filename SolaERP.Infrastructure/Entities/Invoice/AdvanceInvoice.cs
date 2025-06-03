namespace SolaERP.Application.Entities.Invoice;

public class AdvanceInvoice : BaseEntity
{
    public int InvoiceRegisterId { get; set; }
    public string InvoiceNo { get; set; }
    public decimal OriginalAdvanceAmount { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal AllocatedAmount { get; set; }
    public DateTime InvoiceReceivedDate { get; set; }
}