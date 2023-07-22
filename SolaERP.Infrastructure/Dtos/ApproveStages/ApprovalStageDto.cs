using SolaERP.Application.Dtos.ApproveStage;



namespace SolaERP.Application.Dtos.ApproveStages
{
    public class ApprovalStageDto : ApprovalStagesMainDto
    {
        public List<ApprovalStageDetailDto> Details { get; set; }
    }


    public class ApprovalStageDetailDto : ApprovalStagesDetailDto
    {
    }



}


