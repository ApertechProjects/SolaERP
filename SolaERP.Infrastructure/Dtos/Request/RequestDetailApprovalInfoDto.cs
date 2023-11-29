namespace SolaERP.Application.Dtos.Request
{
    public class RequestDetailApprovalInfoDto
    {
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovalStatusName { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Comment { get; set; }
    }
}
