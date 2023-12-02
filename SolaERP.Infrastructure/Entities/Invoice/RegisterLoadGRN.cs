namespace SolaERP.Application.Entities.Invoice
{
    public class RegisterLoadGRN : BaseEntity
    {
        public string Qaime { get; set; }
        public string GRNReference { get; set; }
        public decimal Total { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}
