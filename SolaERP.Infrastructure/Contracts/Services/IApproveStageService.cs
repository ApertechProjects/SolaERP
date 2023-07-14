using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveStageService
    {
        #region Main
        Task<ApiResponse<int>> SaveApproveStageMainAsync(string name, ApprovalStageSaveModel approvalStageSaveVM);
        Task<ApiResponse<ApproveStagesMainDto>> GetMainByIdAsync(int approveStageMainId);
        Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId);
        Task<ApiResponse<List<ApprovalStatusDto>>> GetApproveStatus();
        Task<ApiResponse<bool>> DeleteApproveStageAsync(ApproveStageDeleteModel model);
        Task<ApiResponse<ApprovalStageDto>> GetApprovalStageAsync(int mainId);
        #endregion
        //
    }
}
