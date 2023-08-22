using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Bid
{
    public class BidAll
    {
        public int BidMainId { get; set; }
        public long LineNo { get; set; }
        public string RFQNo { get; set; }
        public string BidNo { get; set; }
        public string OperatorComment { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public string DeliveryTerms { get; set; }
        public string DeliveryTime { get; set; }
        public string PaymentTerms { get; set; }
        public decimal ExpectedCost { get; set; }
        public string Status { get; set; }
        public string ApproveStatus { get; set; }
        public int Emergency { get; set; }
        public string ComparisonNo { get; set; }
        public string EnteredBy { get; set; }
        public string OrderNo { get; set; }
        public DateTime RFQDeadline { get; set; }
    }
}
