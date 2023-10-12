using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class CreateAdvance
    {
        public string TransactionReference { get; set; }
        public string SystemInvoiceNo { get; set; }
        public string InvoiceNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermsName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PaymentRequestAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public string Budget { get; set; }
        public string Department { get; set; }
    }
}
