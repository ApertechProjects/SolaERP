using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonApprovalInformationLoad
    {
        public int Sequence { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public string FullName { get; set; }
        public string ApproveStatus { get; set; }
        public string SignaturePhoto { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Comment { get; set; }
        public string UserPhoto { get; set; }
    }
}
