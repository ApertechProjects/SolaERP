using System.Text.Json.Serialization;

namespace SolaERP.Infrastructure.Models
{
    public class RequestDetailApproveModel
    {
        public List<int> RequestDetailIds { get; set; }
        public int ApproveStatusId { get; set; }
        public string Comment { get; set; }
        public int Sequence { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
