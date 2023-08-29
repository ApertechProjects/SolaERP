namespace SolaERP.Application.Entities.Request
{
    public class RequestDetail : BaseEntity
    {
        public int RequestDetailId { get; set; }
        public int RequestMainId { get; set; }
        public string LineNo { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public DateTime RequestedDate { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string RUOM { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Buyer { get; set; }
        public decimal AvailableInMainStock { get; set; }
        public decimal QuantityFromStock { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal RemainingBudget { get; set; }
        public decimal Amount { get; set; }
        public string ConnectedOrderReference { get; set; }
        public decimal ConnectedOrderLineNo { get; set; }
        public string AccountCode { get; set; }
        public int Condition { get; set; }
        public int ItemCategory { get; set; }
        public int Priority { get; set; }
        public decimal ManualUP { get; set; }
        public bool AlternativeItem { get; set; }
        public int? AnalysisCode1Id { get; set; }
        public int? AnalysisCode2Id { get; set; }
        public int? AnalysisCode3Id { get; set; }
        public int? AnalysisCode4Id { get; set; }
        public int? AnalysisCode5Id { get; set; }
        public int? AnalysisCode6Id { get; set; }
        public int? AnalysisCode7Id { get; set; }
        public int? AnalysisCode8Id { get; set; }
        public int? AnalysisCode9Id { get; set; }
        public int? AnalysisCode10Id { get; set; }
        public int? Catid { get; set; }
    }
}
