using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BaseBidComparisonLoad
    {
        public long RowNum { get; set; }
        public int ApproveStatus { get; set; }
        public int Emergency { get; set; }
        public string SingleSourceReasons { get; set; }
        public int ProcurementType { get; set; }
        public string ComparisonNo { get; set; }
        public string RFQNo { get; set; }
        public string Buyer { get; set; }
        public DateTime Comparisondeadline { get; set; }
        public DateTime RFQDeadline { get; set; }
        public string SpecialistComment { get; set; }

        public T GetChild<T>() where T : BaseBidComparisonLoad, new()
        {
            return new T
            {
                RFQNo = this.RFQNo,
                RFQDeadline = this.RFQDeadline,
                Comparisondeadline = this.Comparisondeadline,
                ComparisonNo = this.ComparisonNo,
                ApproveStatus = this.ApproveStatus,
                Buyer = this.Buyer,
                Emergency = this.Emergency,
                ProcurementType = this.ProcurementType,
                RowNum = this.RowNum,
                SpecialistComment = this.SpecialistComment,
                SingleSourceReasons = this.SingleSourceReasons,
            };
        }
    }
}
