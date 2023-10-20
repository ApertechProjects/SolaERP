using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentOrderPostAudit
    {
        public int JournalNo { get; set; }
        public string SunUser { get; set; }
        public int CurrentPeriod { get; set; }
        public List<ASalfldgDto> ASalfldgs { get; set; }
    }
}
