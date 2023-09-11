using System.Text.Json.Serialization;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidComparisonFilterDto
    {
        public int RFQMainId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
