using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageService
    {
        #region Main
        Task<ApiResponse<ApprovalStageSaveModel>> SaveApproveStageMainAsync(string name, ApprovalStageSaveModel approvalStageSaveVM);
        Task<ApiResponse<ApproveStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approveStageMainId);
        Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId);
        Task<ApiResponse<List<ApprovalStatusDto>>> GetApproveStatus();
        Task<ApiResponse<bool>> DeleteApproveStageAsync(int approveStageMainId);
        #endregion
        //
    }
}
