using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestDetail : BaseEntity
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
        public string ConnectedOrderReference { get; set; }
        public decimal ConnectedOrderLineNo { get; set; }
        public string AccountCode { get; set; }
        public string AnalysisCode1Id { get; set; }
        public string AnalysisCode2Id { get; set; }
        public string AnalysisCode3Id { get; set; }
        public string AnalysisCode4Id { get; set; }
        public string AnalysisCode5Id { get; set; }
        public string AnalysisCode6Id { get; set; }
        public string AnalysisCode7Id { get; set; }
        public string AnalysisCode8Id { get; set; }
        public string AnalysisCode9Id { get; set; }
        public string AnalysisCode10Id { get; set; }
    }
}
