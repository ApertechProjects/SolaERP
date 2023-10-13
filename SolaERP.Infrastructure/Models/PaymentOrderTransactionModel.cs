using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentOrderTransactionModel
    {
        public int PaymentOrderMainId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string BankAccount { get; set; }

    }
}
