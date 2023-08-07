using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Bid
{
    public class BidMainDto
    {
        public int BidMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public int RFQMainId { get; set; }
        public string BidNo { get; set; }
        public string OperatorComment { get; set; }
        public string VendorCode { get; set; }
        public string CurrencyCode { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountValues { get; set; }
        public string DeliveryTerms { get; set; }
        public string DeliveryTime { get; set; }
        public string PaymentTerms { get; set; }
        public decimal ExpectedCost { get; set; }
        public int Status { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApproveStageMainId { get; set; }
        public int UserId { get; set; }
        public int NewBidMainId { get; set; }
        public string NewBidNo { get; set; }
    }
}
