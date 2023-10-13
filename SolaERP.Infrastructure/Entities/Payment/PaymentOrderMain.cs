using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class PaymentOrderMain : BaseEntity
    {
        public int BusinessUnitId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class PaymentOrderDetail : BaseEntity
    {
        public long LineNo { get; set; }
        public int PaymentDocumentDetailId { get; set; }
        public string PaymentRequestNo { get; set; }
        public DateTime PaymentRequestDate { get; set; }
        public string TransactionReference { get; set; }
        public string Reference { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PaymentRequestAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal GRNAmount { get; set; }
        public DateTime SentDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermCode { get; set; }
        public string PaymentTermName { get; set; }
        public int AgingDays { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string Department { get; set; }
    }
}
