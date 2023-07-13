using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class VendorApproveModel
    {
        public int VendorId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        public int ApproveStatusId { get; set; }
        public string Comment { get; set; }
        public int Sequence { get; set; }
    }
}
