using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class CreateBalance : BaseEntity
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
