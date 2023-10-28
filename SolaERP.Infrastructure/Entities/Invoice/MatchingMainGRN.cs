using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
    public class MatchingMainGRN : BaseEntity
    {
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderTotal { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal RNETotal { get; set; }
        public decimal UnmatchedAmount { get; set; }
        public string MatchStatus { get; set; }
    }
}
