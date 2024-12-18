using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonSummary : BaseEntity
    {
        public int BidComparisonSummaryId { get; set; }
        public int BidComparisonId { get; set; }
        public string VendorCode { get; set; }
        public decimal NormalizationCost { get; set; }
    }
}
