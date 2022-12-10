using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;

namespace SolaERP.Infrastructure.ViewModels
{
    public class ApprovalStageSaveVM
    {
        public ApproveStagesMainDto ApproveStagesMainDto { get; set; }
        public List<ApproveStagesDetailDto> ApproveStagesDetailDtos { get; set; }
    }
}
