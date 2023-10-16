using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentOrderPostModel
    {
        public int JournalNo { get; set; }
        public List<PaymentDocumentPost> PaymentDocumentPosts { get; set; }

    }

    public class PaymentDocumentPost
    {
        public int PaymentOrderTransactionId { get; set; }
        public int PaymentOrderMainId { get; set; }
        public int PaymentDocumentDetailId { get; set; }
        public int LineNo { get; set; }
        public string TransactionReference { get; set; }
        public string Reference { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public string AccountCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Period { get; set; }
        public string D_C { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal BaseConvRate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal ReportingConvRate { get; set; }
        public decimal ReportingAmount { get; set; }
        public string JournalType { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string VendorCode { get; set; }
        public string Department { get; set; }
    }

}
