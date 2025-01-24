namespace SolaERP.Application.Dtos.BidComparison
{
    public class BaseBidComparisonLoadDto
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
        public string Status { get; set; }
        public string StatusName { get; set; }
        public DateTime? Comparisondeadline { get; set; }
        public DateTime? RFQDeadline { get; set; }
        public string SpecialistComment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ComparisonDate { get; set; }
        public bool HasAttachments { get; set; }
    }
}