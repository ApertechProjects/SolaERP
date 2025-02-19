#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonBidApproveDto
    {
        public int BidComparisonId { get; set; }
        public int ApproveStatusId { get; set; }
        public int? RejectReasonId { get; set; }
        public string? Comment { get; set; }
    }
}
