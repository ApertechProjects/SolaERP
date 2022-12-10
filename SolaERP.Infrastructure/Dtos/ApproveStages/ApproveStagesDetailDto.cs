namespace SolaERP.Infrastructure.Dtos.ApproveStages
{
    public class ApproveStagesDetailDto
    {
        public int ApproveStageDetailsId { get; set; }
        public int ApproveStageMainId { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
        public List<ApproveStageRoleDto> ApproveStageRolesDto { get; set; }
        public string Type { get; set; }
    }
}
