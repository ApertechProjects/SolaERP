using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonCreateDto
    {
        public int BidComparisonId { get; set; }
        public int RFQMainId { get; set; }
        public string ComparisonNo { get; set; }
        public int ApproveStageMain { get; set; }
        public DateTime ComparisonDate { get; set; }
        public DateTime ComparisonDeadline { get; set; }
        public string SpecialistComment { get; set; }
        public List<int> SingleSourceReasonId { get; set; }
        public int UserId { get; set; }
    }
}
