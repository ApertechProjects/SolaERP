namespace SolaERP.Application.Models
{
    public class PaymentGetModel
    {
        public int BusinessUnitId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string VendorCode { get; set; }
    }


    public class PaymentOrderGetModel : PaymentGetModel
    {
        public string InvoiceNo { get; set; }
        public string Reference { get; set; }
        public string TransactionReference { get; set; }
    }

}
