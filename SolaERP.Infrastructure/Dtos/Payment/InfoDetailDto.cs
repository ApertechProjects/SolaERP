using SolaERP.Application.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Payment
{
    public class InfoDetailDto
    {
        public int PaymentDocumentDetailId { get; set; }
        public int PaymentDocumentMainId { get; set; }
        public string TransactionReference { get; set; }
        public string Reference { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemInvoiceNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermsName { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PaymentRequestAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public string Budget { get; set; }
        public string Employee { get; set; }
        public string WellNo { get; set; }
        public string LinkAccount { get; set; }
        public decimal GRNAmount { get; set; }
        public int AgingDays { get; set; }
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
        public int? InvoiceRegisterDetailId { get; set; }
    }
}
