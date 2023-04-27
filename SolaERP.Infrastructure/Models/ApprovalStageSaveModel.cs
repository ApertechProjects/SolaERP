using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.ApproveStages;

namespace SolaERP.Application.Models
{
    public class ApprovalStageSaveModel
    {
        public ApproveStagesMainDto ApproveStagesMainDto { get; set; }
        public List<ApproveStagesDetailDto> ApproveStagesDetailDtos { get; set; }
        public string Type { get; set; }
    }
}
