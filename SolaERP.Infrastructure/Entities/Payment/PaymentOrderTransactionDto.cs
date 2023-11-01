using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class PaymentOrderTransactionDto
    {
        public long LineNo { get; set; }
        public int PaymentdocumentDetailId { get; set; }
        public string TransactionReference { get; set; }
        public string Reference { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Period { get; set; }
        public string D_C { get; set; }
        public decimal Amount { get; set; }
        public decimal BaseRate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal ReportRate { get; set; }

        public decimal ReportAmount { get; set; }
        public string JournalType { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string VendorCode { get; set; }
        public string Department { get; set; }
        public string CurrencyCode { get; set; }



    }
}
