using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class CreateBalanceModel
    {
        public string VendorCode { get; set; }
        public int BusinessUnitId { get; set; }
        public int Type { get; set; }
    }
}
