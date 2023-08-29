using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonSendToApproveDto
    {
        public int BidComparisonId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
