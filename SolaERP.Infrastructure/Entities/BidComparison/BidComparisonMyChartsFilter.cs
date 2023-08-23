using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonMyChartsFilter
    {
        public int BusinessUnitid { get; set; }
        public string Emergency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public string ApproveStatus { get; set; }
    }
}
