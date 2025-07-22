namespace SolaERP.Application.Dtos.Request
{
    public class RequestDetailsWithAnalysisCodeDto
    {
        public int RequestDetailId { get; set; }
        public int RequestMainId { get; set; }
        public string LineNo { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public DateTime RequestedDate { get; set; }
        public string ItemCode { get; set; }
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
        public int ConnectedOrderLineNo { get; set; }
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
        public int MaxSequence { get; set; }
    }
}
