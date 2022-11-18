using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorRepresentedCompany : BaseEntity
    {
        public int VendorRepresentedCompanyId { get; set; }
        public int VendorId { get; set; }
        public string RepresentedCompanyName { get; set; }
    }
}
