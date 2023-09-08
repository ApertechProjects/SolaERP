using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonBidApprovalsLoadDto
    {
        public int BidMainId { get; set; }
        public int Sequence { get; set; }
        public string ApproveStageName { get; set; }
        public int ApproveStatus { get; set; }
        public int RFQDetailId { get; set; }
        public int BidDetailId { get; set; }
    }
}
