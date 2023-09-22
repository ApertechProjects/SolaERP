using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class CreateOrderModel
    {
        public string VendorCode { get; set; }
        public int BusinessUnitId { get; set; }
        public string CurrencyCode { get; set; }
    }
}
