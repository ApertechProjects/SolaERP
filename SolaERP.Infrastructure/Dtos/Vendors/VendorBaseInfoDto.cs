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
        public decimal SupplierWHTRate { get; set; }
        public int TaxesId { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
    }
}
