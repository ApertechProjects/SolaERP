namespace SolaERP.Application.Dtos.Invoice;

public class AdvanceInvoiceDto
{
    public int InvoiceRegisterId { get; set; }
    public string InvoiceNo { get; set; }
    public decimal InvoiceAmount { get; set; }
    public DateTime InvoiceReceivedDate { get; set; }
}