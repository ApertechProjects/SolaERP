using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class RequestDetailApproveModel
    {
        public List<RequestDetailIds> RequestDetails { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
        public int RejectReasonId { get; set; }
        public string RejectReason { get; set; }
        public string BusinessUnitName { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }


    public class RequestDetailIds
    {
        public int RequestDetailId { get; set; }
        public int? Sequence { get; set; }
    }
}
