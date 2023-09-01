using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.RFQ
{
    public class RFQVendorIUD
    {
        public int Id { get; set; } 
        public List<string> VendorCodes { get; set; }

    }
}
