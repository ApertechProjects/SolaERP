using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BaseBidComparisonLoadDto
    {
        public long RowNum { get; set; }
        public string ApproveStatus { get; set; }
        public string Emergency { get; set; }
        public string SingleSourceReasons { get; set; }
        public string ProcurementType { get; set; }
        public string ComparisonNo { get; set; }
        public string RFQNo { get; set; }
        public string Buyer { get; set; }
        public DateTime Comparisondeadline { get; set; }
        public DateTime RFQDeadline { get; set; }
        public string SpecialistComment { get; set; }
    }
}
