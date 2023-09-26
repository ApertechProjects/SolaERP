using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentChangeStatusModel
    {
        public int PaymentDocumentMainId { get; set; }
        public int Sequence { get; set; }
        public int ApproveStatus { get; set; }

    }
}
