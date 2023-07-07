namespace SolaERP.Application.Dtos.ApproveStages
{
    public class ApproveStagesDetailDto
    {
        public int Id { get; set; }
        public int ApproveStageMainId { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
        public bool Skip { get; set; }
        public int SkipDays { get; set; }
        public bool BackToInitiatorOnReject { get; set; }
        public List<ApproveStageRoleDto> ApproveStageRoles { get; set; }
        public string Type { get; set; }
    }
}
