using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.ApproveStage
{
    public class ApprovalStagesDetail : BaseEntity
    {
        public int ApproveStageDetailsId { get; set; }
        public int ApproveStageMainId { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
        public bool Skip { get; set; }
        public int SkipDays { get; set; }
        public bool BackToInitiatorOnReject { get; set; }
        [DbIgnore]
        public List<ApprovalStageRole> ApproveStageRoles { get; set; }
    }
}
