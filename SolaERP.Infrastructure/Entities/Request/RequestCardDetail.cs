namespace SolaERP.Application.Entities.Request
{
    public class RequestCardDetail : BaseEntity
    {
        public int RequestDetailId { get; set; }
        public int RequestMainId { get; set; }
        public string LineNo { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public DateTime RequestedDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public decimal AvailableInMainStock { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Buyer { get; set; }
        public string BuyerName { get; set; }
        public decimal AvailableQuantity { get; set; } //
        public decimal QuantityFromStock { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal RemainingBudget { get; set; }
        public decimal Amount { get; set; }
        public string ConnectedOrderReference { get; set; }
        public decimal ConnectedOrderLineNo { get; set; }
        public string AccountCode { get; set; }
        public bool AlternativeItem { get; set; }
        public decimal ManualUP { get; set; }
        public int CatId { get; set; }
        public decimal LastUnitPrice { get; set; }
        public string LastPurchaseDate { get; set; }
        public int LastVendor { get; set; }
        public int RequestAnalysisId { get; set; }
        public int AnalysisCode1Id { get; set; }
        public int AnalysisCode2Id { get; set; }
        public int AnalysisCode3Id { get; set; }
        public int AnalysisCode4Id { get; set; }
        public int AnalysisCode5Id { get; set; }
        public int AnalysisCode6Id { get; set; }
        public int AnalysisCode7Id { get; set; }
        public int AnalysisCode8Id { get; set; }
        public int AnalysisCode9Id { get; set; }
        public int AnalysisCode10Id { get; set; }
        public string StatusName { get; set; }
        public string ApproveStatusName { get; set; }
    }
}
