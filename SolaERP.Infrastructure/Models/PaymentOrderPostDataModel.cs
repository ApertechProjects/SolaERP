using SolaERP.Application.Dtos.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentOrderPostDataModel
    {
        public int JournalNo { get; set; }
        List<ASalfldgDto> ASalfldgs { get; set; }
    }


}
