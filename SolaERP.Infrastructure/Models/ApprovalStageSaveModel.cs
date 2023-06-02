using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.ApproveStages;

namespace SolaERP.Application.Models
{
    public class ApprovalStageSaveModel
    {
        public ApproveStageMainInputModel ApproveStagesMain { get; set; }
        public List<ApproveStageDetailInputModel> ApproveStagesDetailDtos { get; set; }
        public string Type { get; set; }
    }
}
