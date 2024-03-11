namespace SolaERP.Application.Models
{
    public class PaymentRequestGetModel
    {
        public string VendorCode { get; set; }
        public string CurrencyCode { get; set; }
        public int BusinessUnitId { get; set; }
    }
}