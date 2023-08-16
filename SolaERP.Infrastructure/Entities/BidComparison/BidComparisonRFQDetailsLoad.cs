using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonRFQDetailsLoad
    {
        public int LineNo { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public decimal Budget { get; set; }
        public decimal RemainingBudget { get; set; }    
    }
}
