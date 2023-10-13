using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentOrderParamModel
    {
        public int PaymentOrderMainId { get; set; }
        public List<int> PaymentDocumentMainIds { get; set; }
    }

    public class PaymentOrderLoadModel
    {
        public PaymentOrderMainDto Main { get; set; }
        public List<PaymentOrderDetailDto> Details { get; set; }
    }
}
