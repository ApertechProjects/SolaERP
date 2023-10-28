using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Payment
{
    public class PaymentOrderDto
    {
        public int PaymentOrderMainId { get; set; }
        public long LineNo { get; set; }
        public string PaymentOrderNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentTerms { get; set; }
        public string Comment { get; set; }
        public int JournalNo { get; set; }
        public decimal Base { get; set; }
        public decimal Reporting { get; set; }
        public decimal BankCharge { get; set; }
    }
}