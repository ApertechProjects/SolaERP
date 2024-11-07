using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
    public class InvoiceRegisterPayablesTransactions : BaseEntity
    {
        public int InvoiceLineNo { get; set; }
        public string AccountCode { get; set; }
        public int Period { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string D_C { get; set; } = "D";
        public string JournalType { get; set; } = "PINV";
        public string TransactionReference { get; set; }
        public string LineDescription { get; set; }
        public string CurrencyCode { get; set; }
        public decimal BaseRate { get; set; }
        public decimal BaseAmount { get; set; }
        public string LinkAccount { get; set; }
        public decimal ReportingRate { get; set; }
        public decimal ReportingAmount { get; set; }
        public int LinkReference { get; set; }
        public string Comment { get; set; }
        public string InvoiceNo { get; set; }
        public string ReferenceDocNo { get; set; }
        public string Analysis1 { get; set; }
        public string Analysis2 { get; set; }
        public string Analysis3 { get; set; }
        public string Analysis4 { get; set; }
        public string Analysis5 { get; set; }
        public string Analysis6 { get; set; }
        public string Analysis7 { get; set; }
        public string Analysis8 { get; set; }
        public string Analysis9 { get; set; }
        public string Analysis10 { get; set; }
    }
}
