using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonBidDetailsLoad
    {
        public int BidMainId { get; set; }
        public string BidNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountValue { get; set; }
        public bool AlternativeItem { get; set; }
        public string AlternativeDescription { get; set; }
    }
}
