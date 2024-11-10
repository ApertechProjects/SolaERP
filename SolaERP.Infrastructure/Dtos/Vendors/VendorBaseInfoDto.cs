using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Vendors
{
    public class VendorBaseInfoDto
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public int TaxesId { get; set; }
        public int WithHoldingTaxId { get; set; }
        public decimal Tax { get; set; }
    }
}
