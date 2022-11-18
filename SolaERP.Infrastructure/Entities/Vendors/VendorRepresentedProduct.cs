using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorRepresentedProduct :BaseEntity
    {
        public int VendorRepresentedProductId { get; set; }
        public int VendorId { get; set; }
        public string RepresentedProductName { get; set; }
    }
}
