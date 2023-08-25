using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonSingleSourceReasonsLoad
    {
        public bool Checked { get; set; }
        public int SingleSourcereasonId { get; set; }
        public string SingleSourceReason { get; set; }
        public int Other { get; set; }
    }
}
