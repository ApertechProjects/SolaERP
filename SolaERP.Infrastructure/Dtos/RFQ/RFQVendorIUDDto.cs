using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.RFQ
{
    public class RFQVendorIUDDto
    {
        public int Id { get; set; }
        public List<string> VendorCodes { get; set; }
    }
}
