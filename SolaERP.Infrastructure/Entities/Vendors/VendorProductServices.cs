using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorProductServices : BaseEntity
    {
        public int VendorProductService { get; set; }
        public int VendorId { get; set; }
        public int ProductServiceId { get; set; }
    }
}
