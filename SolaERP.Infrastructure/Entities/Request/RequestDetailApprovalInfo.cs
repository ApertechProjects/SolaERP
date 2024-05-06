namespace SolaERP.Application.Entities.Request
{
    public class RequestDetailApprovalInfo : BaseEntity
    {
        public int RequestApprovalId { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
        public string FullName { get; set; }
        public string ApprovalStatusName { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Comment { get; set; }
    }
}
