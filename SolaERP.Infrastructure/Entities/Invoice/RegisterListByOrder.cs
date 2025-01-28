namespace SolaERP.Application.Entities.Invoice
{
    public class RegisterListByOrder : BaseEntity
    {
        public int InvoiceRegisterId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public string InvoiceComment { get; set; }
        public decimal OrderTotal { get; set; }
        // public string WithHoldingTaxCode { get; set; }
    }
}
