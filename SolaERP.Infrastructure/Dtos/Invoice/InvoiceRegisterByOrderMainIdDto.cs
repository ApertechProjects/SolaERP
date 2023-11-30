using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Invoice
{
    public class InvoiceRegisterByOrderMainIdDto
    {
        public int InvoiceRegisterId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public string InvoiceComment { get; set; }
        public decimal OrderTotal { get; set; }
        public string WithHoldingTaxCode { get; set; }
    }
}
