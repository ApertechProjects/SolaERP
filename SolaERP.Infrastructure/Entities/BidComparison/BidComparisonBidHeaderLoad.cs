using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonBidHeaderLoad
    {
        public int BidMainId { get; set; }
        public string BidNo { get; set; }
        public string VendorName { get; set; }
        public bool Discualified { get; set; }
        public string DiscualificationReason { get; set; }
        public string DeliveryTerms { get; set; }
        public string DeliveryTime { get; set; }
        public string PaymentTerms { get; set; }
        public decimal ExpectedCost { get; set; }
        public decimal BudgetBalance { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal ReportingAmount { get; set; }
        public string BaseCurrencyCode { get; set; }
        public string ReportingCurrencyCode { get; set; }
    }
}
