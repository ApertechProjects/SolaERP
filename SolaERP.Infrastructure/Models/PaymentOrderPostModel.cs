using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentOrderPostModel
    {
        public string BusinessUnitCode { get; set; }
        public int JournalNo { get; set; }
        public int AllocationReference { get; set; }
        public List<PaymentDocumentPost> PaymentDocumentPosts { get; set; }
        public PaymentOrderPostMain PaymentOrderMain { get; set; }
        public List<PaymentOrderPostDetail> PaymentOrderDetails { get; set; }

    }

    public class PaymentOrderPostMain
    {
        public int PaymentOrderMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public string PaymentOrderNo { get; set; }
        public string VendorCode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string BankAccount { get; set; }
        public decimal BankCharge { get; set; }
        public string BankChargeAccount { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
    }

    public class PaymentOrderPostDetail
    {
        public int PaymentOrderDetailId { get; set; }
        public int PaymentDocumentDetailId { get; set; }
        public decimal Amount { get; set; }
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
        public decimal ReportRate { get; set; }
        public decimal ReportAmount { get; set; }
        public string JournalType { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string VendorCode { get; set; }
        public string Department { get; set; }
    }

    public class PaymentTransaction
    {
        public int PaymentOrderTransactionId { get; set; }
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

    public class PaymentOrderPostMainSaveResult
    {
        public int PaymentOrderMainId { get; set; }
        public string PaymentOrderNo { get; set; }
    }

    public class PaymentOrderPostDataResult
    {
        public int JournalNo { get; set; }
        public string PaymentOrderNo { get; set; }
        public int PaymentOrderMainId { get; set; }

    }
}
