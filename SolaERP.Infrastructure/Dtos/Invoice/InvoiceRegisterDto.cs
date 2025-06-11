namespace SolaERP.Application.Dtos.Invoice;

public class InvoiceRegisterDto 
{
    public int InvoiceRegisterId { get; set; }
    public string InvoiceNo { get; set; }
    public string OrderNo { get; set; }
    public string VendorCode { get; set; }
    public string VendorName { get; set; }
    public decimal InvoiceAmount { get; set; }
}