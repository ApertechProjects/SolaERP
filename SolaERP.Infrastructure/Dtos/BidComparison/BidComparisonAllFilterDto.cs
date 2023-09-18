using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonAllFilterDto
    {
        public int BusinessUnitId { get; set; }
        public List<int> Emergency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<int> Status { get; set; }
        public List<int> ApproveStatus { get; set; }
    }
}
