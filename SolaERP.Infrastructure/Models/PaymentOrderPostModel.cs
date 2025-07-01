using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SolaERP.Application.Models
{
    public class PaymentOrderPostModel
    {
        public int BusinessUnitId { get; set; }
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
        [JsonProperty("ReportRate")] public decimal ReportingConvRate { get; set; }
        [JsonProperty("ReportingAmount")] public decimal ReportinAmount { get; set; }
        public string JournalType { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string VendorCode { get; set; }
        public string Department { get; set; }
        public string AnalysisCode1 { get; set; }
        public string AnalysisCode2 { get; set; }
        public string AnalysisCode3 { get; set; }
        public string AnalysisCode4 { get; set; }
        public string AnalysisCode5 { get; set; }
        public string AnalysisCode6 { get; set; }
        public string AnalysisCode7 { get; set; }
        public string AnalysisCode8 { get; set; }
        public string AnalysisCode9 { get; set; }
        public string AnalysisCode10 { get; set; }
        public string GNRL_DESCR_01 { get; set; }
        public string GNRL_DESCR_02 { get; set; }
        public string GNRL_DESCR_03 { get; set; }
        public string GNRL_DESCR_04 { get; set; }
        public string GNRL_DESCR_05 { get; set; }
        public string GNRL_DESCR_06 { get; set; }
        public string GNRL_DESCR_07 { get; set; }
        public string GNRL_DESCR_08 { get; set; }
        public string GNRL_DESCR_09 { get; set; }
        public string GNRL_DESCR_10 { get; set; }
        public string GNRL_DESCR_11 { get; set; }
        public string GNRL_DESCR_12 { get; set; }
        public string GNRL_DESCR_13 { get; set; }
        public string GNRL_DESCR_14 { get; set; }
        public string GNRL_DESCR_15 { get; set; }
        public string GNRL_DESCR_16 { get; set; }
        public string GNRL_DESCR_17 { get; set; }
        public string GNRL_DESCR_18 { get; set; }
        public string GNRL_DESCR_19 { get; set; }
        public string GNRL_DESCR_20 { get; set; }
        public string GNRL_DESCR_21 { get; set; }
        public string GNRL_DESCR_22 { get; set; }
        public string GNRL_DESCR_23 { get; set; }
        public string GNRL_DESCR_24 { get; set; }
        public string GNRL_DESCR_25 { get; set; }
        public DateTime? GNRL_1_DATETIME { get; set; }
        public DateTime? GNRL_2_DATETIME { get; set; }
        public DateTime? GNRL_3_DATETIME { get; set; }
        public DateTime? GNRL_4_DATETIME { get; set; }
        public DateTime? GNRL_5_DATETIME { get; set; }
        public string LINK_REF_1 { get; set; }
    }
    
    public class PaymentOrderPostDataDto
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
         public decimal ReportinAmount { get; set; }
        public string JournalType { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string VendorCode { get; set; }
        public string Department { get; set; }
        public string AnalysisCode1 { get; set; }
        public string AnalysisCode2 { get; set; }
        public string AnalysisCode3 { get; set; }
        public string AnalysisCode4 { get; set; }
        public string AnalysisCode5 { get; set; }
        public string AnalysisCode6 { get; set; }
        public string AnalysisCode7 { get; set; }
        public string AnalysisCode8 { get; set; }
        public string AnalysisCode9 { get; set; }
        public string AnalysisCode10 { get; set; }
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
        public decimal ReportinAmount { get; set; }
        public string JournalType { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public string VendorCode { get; set; }
        public string Department { get; set; }
        public string? AnalysisCode1 { get; set; }
        public string? AnalysisCode2 { get; set; }
        public string? AnalysisCode3 { get; set; }
        public string? AnalysisCode4 { get; set; }
        public string? AnalysisCode5 { get; set; }
        public string? AnalysisCode6 { get; set; }
        public string? AnalysisCode7 { get; set; }
        public string? AnalysisCode8 { get; set; }
        public string? AnalysisCode9 { get; set; }
        public string? AnalysisCode10 { get; set; }
        public string GNRL_DESCR_01 { get; set; }
        public string GNRL_DESCR_02 { get; set; }
        public string GNRL_DESCR_03 { get; set; }
        public string GNRL_DESCR_04 { get; set; }
        public string GNRL_DESCR_05 { get; set; }
        public string GNRL_DESCR_06 { get; set; }
        public string GNRL_DESCR_07 { get; set; }
        public string GNRL_DESCR_08 { get; set; }
        public string GNRL_DESCR_09 { get; set; }
        public string GNRL_DESCR_10 { get; set; }
        public string GNRL_DESCR_11 { get; set; }
        public string GNRL_DESCR_12 { get; set; }
        public string GNRL_DESCR_13 { get; set; }
        public string GNRL_DESCR_14 { get; set; }
        public string GNRL_DESCR_15 { get; set; }
        public string GNRL_DESCR_16 { get; set; }
        public string GNRL_DESCR_17 { get; set; }
        public string GNRL_DESCR_18 { get; set; }
        public string GNRL_DESCR_19 { get; set; }
        public string GNRL_DESCR_20 { get; set; }
        public string GNRL_DESCR_21 { get; set; }
        public string GNRL_DESCR_22 { get; set; }
        public string GNRL_DESCR_23 { get; set; }
        public string GNRL_DESCR_24 { get; set; }
        public string GNRL_DESCR_25 { get; set; }
        public DateTime? GNRL_1_DATETIME { get; set; }
        public DateTime? GNRL_2_DATETIME { get; set; }
        public DateTime? GNRL_3_DATETIME { get; set; }
        public DateTime? GNRL_4_DATETIME { get; set; }
        public DateTime? GNRL_5_DATETIME { get; set; }
        public string LINK_REF_1 { get; set; }
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