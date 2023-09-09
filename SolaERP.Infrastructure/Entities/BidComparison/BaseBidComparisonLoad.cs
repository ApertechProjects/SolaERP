namespace SolaERP.Application.Entities.BidComparison
{
    public class BaseBidComparisonLoad
    {
        public int? BidComparisonId { get; set; }
        public int RFQMainId { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime ComparisonDate { get; set; }

        public T GetChild<T>() where T : BaseBidComparisonLoad, new()
        {
            return new T
            {
                BidComparisonId = this.BidComparisonId,
                RFQMainId = this.RFQMainId,
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
                CreatedBy = this.CreatedBy,
                ComparisonDate = this.ComparisonDate
            };
        }
    }
}