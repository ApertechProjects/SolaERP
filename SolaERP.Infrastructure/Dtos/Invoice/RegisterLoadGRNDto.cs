namespace SolaERP.Application.Dtos.Invoice
{
    public class RegisterLoadGRNDto
    {
        public int InvoiceMatchingGRNId { get; set; }
        public string GRNReference { get; set; }
        public bool Checked { get; set; }
    }
}
