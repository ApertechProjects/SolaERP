using SolaERP.Business.Dtos.EntityDtos.ApprovalStage;

namespace SolaERP.Business.Dtos.Wrappers
{
    public class SaveApprovalStageWrapper
    {
        public ApproveStageHeader_Load ApproveStageHeader_Load { get; set; }
        public List<ApproveStageDetails_Load> ApproveStageDetails_Load { get; set; }
    }
}
