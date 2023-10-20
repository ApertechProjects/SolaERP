namespace SolaERP.Application.Entities.Invoice
{
    public class RegisterListByOrder : BaseEntity
    {
        public int InvoiceRegisterId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
    }
}
