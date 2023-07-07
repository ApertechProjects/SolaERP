namespace SolaERP.Application.Entities.ApproveStage
{
    public class ApproveStageRole : BaseEntity
    {
        public int Id { get; set; }
        public int ApproveStageDetailId { get; set; }
        public int ApproveRoleId { get; set; }
        public string ApproveRoleName { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
    }
}
