namespace SolaERP.Application.Models
{
    public class PaymentGetModel
    {
        public int BusinessUnitId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string VendorCode { get; set; }
    }
}
