using SolaERP.Application.Entities.RFQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class RfqDetailsSaveRequestModel
    {
        public int RFQMainId { get; set; }
        public List<RfqDetail> Details { get; set; }
    }
}
