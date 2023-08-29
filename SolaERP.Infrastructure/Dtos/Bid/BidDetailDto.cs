using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Bid
{
    public class BidDetailDto
    {
        public int BidDetailId { get; set; }
        public int BidMainId { get; set; }
        public int RFQDetailId { get; set; }
        public int LineNo { get; set; }
        public string ItemCode { get; set; }
        public bool AlternativeItem { get; set; }
        public string AlternativeItemCode { get; set; }
        public bool AlternativeItemName { get; set; }
        public string PUOM { get; set; }
        public decimal ConversionRate { get; set; }
        public string LineDescription { get; set; }
        public int Condition { get; set; }
        public decimal Quantity { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SingleUnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal ConvertedQTY { get; set; }
        public decimal ConvertedUnitPrice { get; set; }
    }
}
