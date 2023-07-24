using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class RfqFilter
    {
        public int BusinessUnitId { get; set; }
        public string ItemCode { get; set; }
        public string Emergency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string RFQType { get; set; }
        public string ProcurementType { get; set; }
    }
}
