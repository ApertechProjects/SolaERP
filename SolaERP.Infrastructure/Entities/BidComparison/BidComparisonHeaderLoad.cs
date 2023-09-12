using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonHeaderLoad
    {
        public int BidComparisonId { get; set; }
        public int RFQMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public string RFQNo { get; set; }
        public int BiddingType { get; set; }
        public int ProcurementType { get; set; }
        public DateTime? RFQDate { get; set; }
        public DateTime? RFQDeadline { get; set; }
        public int Emergency { get; set; }
        public string EnteredBy { get; set; }
        public DateTime? Entrydate { get; set; }
        public string ComparisonNo { get; set; }
        public DateTime ComparisonDate { get; set; }
        public DateTime? Comparisondeadline { get; set; }
        public DateTime RequiredOnSiteDate { get; set; }
        public DateTime DesiredDeliveryDate { get; set; }
        public string Buyer { get; set; }
        public int ApproveStageMain { get; set; }
        public int Status { get; set; }
        public int ApproveStatus { get; set; }
        public string SpecialistComment { get; set; }
    }
}
