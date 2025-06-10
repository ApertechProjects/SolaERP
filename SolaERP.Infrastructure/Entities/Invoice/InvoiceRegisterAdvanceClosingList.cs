namespace SolaERP.Application.Entities.Invoice;

public class InvoiceRegisterAdvanceClosingList : BaseEntity
{
    public int InvoiceregisterId { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime Date { get; set; }
    public string AccountCode { get; set; }
    public string VendorCode { get; set; }
    public string VendorName { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal NotAllocatedAmount { get; set; }
}