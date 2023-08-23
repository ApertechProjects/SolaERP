using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonMyChartsFilterDto
    {
        public int BusinessUnitid { get; set; }
        public List<int> Emergency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<int> Status { get; set; }
        public List<int> ApproveStatus { get; set; }
    }
}
