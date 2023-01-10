namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestDetail : BaseEntity
    {
        public int RequestDetailId { get; set; }
        public int RequestMainId { get; set; }
        public int LineNo { get; set; }
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
        public string ConnectedOrderReference { get; set; }
        public decimal ConnectedOrderLineNo { get; set; }
        public string AccountCode { get; set; }
    }
}
