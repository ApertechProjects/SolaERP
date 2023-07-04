using SolaERP.Application.Dtos.ApproveStage;



namespace SolaERP.Application.Dtos.ApproveStages
{
    public class ApprovalStageDto : ApproveStagesMainDto
    {
        public List<ApprovalStageDetailDto> Details { get; set; }
    }


    public class ApprovalStageDetailDto : ApproveStagesDetailDto
    {
        public List<ApprovalStageRoleDto> Roles { get; set; }
    }


    public class ApprovalStageRoleDto : ApproveStageRoleDto { }
}


