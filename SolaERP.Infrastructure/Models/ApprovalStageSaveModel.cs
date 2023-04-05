using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;

namespace SolaERP.Infrastructure.Models
{
    public class ApprovalStageSaveModel
    {
        public ApproveStagesMainDto ApproveStagesMainDto { get; set; }
        public List<ApproveStagesDetailDto> ApproveStagesDetailDtos { get; set; }
        public string Type { get; set; }
    }
}
