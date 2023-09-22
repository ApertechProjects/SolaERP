using SolaERP.Application.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Payment
{
    public class CreateBalanceDto
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
