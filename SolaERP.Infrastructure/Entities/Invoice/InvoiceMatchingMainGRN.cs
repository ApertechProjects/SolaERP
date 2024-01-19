using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
    public class InvoiceMatchingMainGRN : BaseEntity
    {
        public long LineNo { get; set; }
        public int InvoiceMatchingMainId { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderAmount { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal GRNAmount { get; set; }
        public string Currency { get; set; }
    }
}
