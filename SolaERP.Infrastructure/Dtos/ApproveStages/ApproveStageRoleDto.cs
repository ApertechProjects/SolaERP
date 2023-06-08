namespace SolaERP.Application.Dtos.ApproveStages
{
    public class ApproveStageRoleDto
    {
        public int ApproveStageRoleId { get; set; }
        public int ApproveStageDetailId { get; set; }
        public int ApproveRoleId { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public string Type { get; set; }
    }
}
