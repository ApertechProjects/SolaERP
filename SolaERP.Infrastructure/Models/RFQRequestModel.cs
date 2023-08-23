using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class RFQRequestModel
    {
        public int BusinessUnitId { get; set; }
        public List<int> BusinessCategoryIds { get; set; }
        public string Buyer { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
