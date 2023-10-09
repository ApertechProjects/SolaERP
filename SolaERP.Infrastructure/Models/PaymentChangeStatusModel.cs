using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentChangeStatusModel
    {
        public List<Payment> Payments { get; set; }
        public int ApproveStatus { get; set; }

    }

    public class Payment
    {
        public int PaymentDocumentMainId { get; set; }
        public int Sequence { get; set; }
    }
}
