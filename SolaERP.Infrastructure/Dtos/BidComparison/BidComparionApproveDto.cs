using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonApproveDto
    {
        public int BidMainId { get; set; }
        public int Sequence { get; set; }
        public int ApproveStatus { get; set; }
        public int RFQDeatilid { get; set; }
    }
}
