using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Payment
{
    public class AllocationDataDto : ASalfldgDto
    {
        public int Action { get; set; }
        public string GNRL_DESCR_24 { get; set; }
        public string GNRL_DESCR_25 { get; set; }
    }
}
