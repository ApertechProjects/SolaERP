using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class InfoDetail : BaseEntity
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
        public string PaymentTermName { get; set; }
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
    }
}