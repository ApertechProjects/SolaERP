using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Invoice
{
    public class InvoiceRegisterByOrderMainIdDto
    {
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public string Currency { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public int WithHoldingTaxId { get; set; }
        public string WithHoldingTaxCode { get; set; }
        public decimal OrderTotal { get; set; }
        public List<Dtos.TaxDto.TaxDto> TaxDatas { get; set; }
        //public DateTime InvoiceReceivedDate { get; set; }

    }
}
