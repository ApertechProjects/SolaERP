using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonFilterDto
    {
        public int BidComparisonId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
