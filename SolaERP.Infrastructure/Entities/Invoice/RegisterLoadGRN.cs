namespace SolaERP.Application.Entities.Invoice
{
    public class RegisterLoadGRN : BaseEntity
    {
        public int InvoiceMatchingGRNId { get; set; }
        public string GRNReference { get; set; }
        public bool Checked { get; set; }
    }
}
