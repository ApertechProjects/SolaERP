using System.ComponentModel.DataAnnotations.Schema;

namespace SolaERP.Infrastructure.Models
{
    public class RequestDetailSendToApproveModel
    {
        public int RequestDetailId { get; set; }
        public int ApproveStatusId { get; set; }
        public string Comment { get; set; }
        public int Sequence { get; set; }

        [NotMapped]
        public int UserId { get; set; }
    }
}
