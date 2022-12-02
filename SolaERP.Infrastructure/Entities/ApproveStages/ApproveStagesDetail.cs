namespace SolaERP.Infrastructure.Entities.ApproveStage
{
    public class ApproveStagesDetail : BaseEntity
    {
        public int ApproveStageDetailsId { get; set; }
        public int ApproveStageMainId { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
    }
}
