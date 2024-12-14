using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonBidHeaderFilter
    {
        public int RFQMainId { get; set; }
        public int? BidComparisonId { get; set; }
        public int UserId { get; set; }
    }
}
