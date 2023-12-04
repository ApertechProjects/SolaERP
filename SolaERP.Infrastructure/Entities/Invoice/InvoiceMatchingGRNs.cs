namespace SolaERP.Application.Entities.Invoice;

public class InvoiceMatchingGRNs : BaseEntity
{
    public int InvoiceMatchingMainId { get; set; }
    public List<RNEInvoicesMatchingType> RNEInvoicesMatchingTypeList { get; set; }
}

public class RNEInvoicesMatchingType
{
    public string RCVN_TXN_REF { get; set; }
    public DateTime ReceiptDate { get; set; }
}