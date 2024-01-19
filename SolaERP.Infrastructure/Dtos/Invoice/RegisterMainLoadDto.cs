using SolaERP.Application.Entities.Invoice;

namespace SolaERP.Application.Dtos.Invoice
{


    public class RegisterMainLoadDto
    {
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public string VendorCode { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceComment { get; set; }
        public string Comment { get; set; }
    }

}
