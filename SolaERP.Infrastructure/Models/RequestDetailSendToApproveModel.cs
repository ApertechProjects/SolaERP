using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class RequestDetailApproveModel
    {
        public int RequestDetailId { get; set; }
        public int ApproveStatusId { get; set; }
        public string Comment { get; set; }
        public int Sequence { get; set; }
        public int RejectReasonId { get; set; }
        //public string RequestNo { get; set; }
        public string RejectReason { get; set; }
        public string BusinessUnitName { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
