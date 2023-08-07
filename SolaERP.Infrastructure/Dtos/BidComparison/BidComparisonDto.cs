using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.RFQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonDto
    {
        public RFQMainDto RfqMain { get; set; }

        public List<BidDto> Bids { get; set; }
    }
}
