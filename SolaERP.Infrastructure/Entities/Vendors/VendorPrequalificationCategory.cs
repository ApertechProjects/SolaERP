using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorPrequalificationCategory : BaseEntity
    {
        public int VendorPrequalificationCategoryId { get; set; }
        public int VendorId { get; set; }
        public int PrequalificationCategoryId { get; set; }

    }
}
