using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentInfoModel
    {
        public InfoHeaderDto InfoHeader { get; set; }
        public List<InfoDetailDto> InfoDetail { get; set; }
        public List<InfoApproval> InfoApproval { get; set; }
    }
}
