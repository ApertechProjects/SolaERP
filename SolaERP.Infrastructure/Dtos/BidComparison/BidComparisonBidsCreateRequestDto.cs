using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SolaERP.Application.Models;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonBidsCreateRequestDto
    {
        public int BidComparisonId { get; set; }
        public List<BidComparisonBidCreateDto> Bids { get; set; }
        [JsonIgnore] 
        public int UserId { get; set; }
    }
}