using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class BankAccountList : BaseEntity
    {
        public string BankAccount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }
}
