using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentOperationModel
    {
        public List<int> PaymentDocumentMainIds { get; set; }
        public int ApproveStatus { get; set; }
    }


}
