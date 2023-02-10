namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestDetailWithAnalysisCode : BaseEntity
    {
        public int RequestDetailId { get; set; }
        public int RequestMainId { get; set; }
        public int LineNo { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public DateTime RequestedDate { get; set; }
        public int ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Buyer { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal QuantityFromStock { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal RemainingBudget { get; set; }
        public decimal Amount { get; set; }
        public string ConnectedOrderReferance { get; set; }
        public int ConnectecOrderLineNo { get; set; }
        public string AccountCode { get; set; }
        public int RequestAnalysisCode1Id { get; set; }
        public int RequestAnalysisCode2Id { get; set; }
        public int RequestAnalysisCode3Id { get; set; }
        public int RequestAnalysisCode4Id { get; set; }
        public int RequestAnalysisCode5Id { get; set; }
        public int RequestAnalysisCode6Id { get; set; }
        public int RequestAnalysisCode7Id { get; set; }
        public int RequestAnalysisCode8Id { get; set; }
        public int RequestAnalysisCode9Id { get; set; }
        public int RequestAnalysisCode10Id { get; set; }

    }
}
