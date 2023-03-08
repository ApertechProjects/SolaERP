namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestApprovalInfoDto
    {
        public int Sequence { get; set; }
        public DateTime ApproveDate { get; set; }
        public int UserId { get; set; }
        public string ApprovedBy { get; set; }
        public string Comment { get; set; }
        public int ApproveStatus { get; set; }
        public string ApprovalStatusName { get; set; }
    }
}
