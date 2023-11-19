namespace SolaERP.Application.Entities.Invoice;

public class InvoiceMatchingAdvance : BaseEntity
{
    public int InvoiceMatchingMainid { get; set; }
    public int InvoiceRegisterId { get; set; }
    public List<AdvanceInvoicesMatchingType> AdvanceInvoicesMatchingTypeList { get; set; }
}

public class AdvanceInvoicesMatchingType
{
    public int AdvanceInvoiceRegisterId { get; set; }
    public decimal AllocatedAmount { get; set; }
}