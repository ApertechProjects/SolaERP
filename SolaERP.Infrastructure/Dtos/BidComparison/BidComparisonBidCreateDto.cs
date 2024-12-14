using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SolaERP.Application.Models;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonBidCreateDto
    {
        public int BidComparisonBidId { get; set; }
        public int RFQDetailId { get; set; }
        public int BidDetailId { get; set; }
        public decimal Quantity { get; set; }
        public int Status { get; set; }
        public int ApproveStatus { get; set; }
        public bool IsSelected { get; set; }
    }
}