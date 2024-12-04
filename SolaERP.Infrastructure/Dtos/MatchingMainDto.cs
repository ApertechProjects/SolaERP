namespace SolaERP.Application.Dtos;

public class MatchingMainDto
{
    public int OrderMainId { get; set; }
    public string OrderNo { get; set; }
    public string VendorCode { get; set; }
    public string VendorName { get; set; }
    public string Currency { get; set; }
    public string OrderComment { get; set; }
    public string WithHoldingTaxCode { get; set; }
    public int BusinessUnitId { get; set; }
    public int InvoiceRegisterId { get; set; }
    public decimal WHT { get; set; }
    public string Comment { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal OrderAmount { get; set; }
    public decimal SupplierWHTRate { get; set; }
}