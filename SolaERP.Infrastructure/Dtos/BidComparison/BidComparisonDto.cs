using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Entities.BidComparison;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonDto
    {
        public BidComparisonHeaderLoadDto BidComparisonHeader { get; set; }
        public List<BidComparisonBidHeaderLoadDto> Bids { get; set; }
        public List<BidComparisonRFQDetailsLoadDto> RfqDetails { get; set; }

    }
}
