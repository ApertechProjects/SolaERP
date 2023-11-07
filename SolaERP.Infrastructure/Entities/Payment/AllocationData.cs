using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class AllocationData : ASalfldgAndAllocation
    {
        public int Action { get; set; }
        public string GNRL_DESCR_24 { get; set; }
        public string GNRL_DESCR_25 { get; set; }
    }
}