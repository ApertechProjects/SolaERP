using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageService : ILoggableCrudService<ApproveStagesMainDto>
    {
        #region Main
        Task<ApiResponse<ApprovalStageSaveVM>> SaveApproveStageMainAsync(string authToken, ApprovalStageSaveVM approvalStageSaveVM);
        Task<ApiResponse<ApproveStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approveStageMainId);
        Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId);
        Task<ApiResponse<List<AprroveSatusDto>>> GetApproveStatuses();
        #endregion
        //
        #region Details
        Task<int> SaveApproveStageDetailsAsync(ApproveStagesDetailDto details);
        Task<bool> RemoveApproveStageDetailsAsync(int approveStageDetailsId);
        Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
        #endregion
        //
        #region Roles
        Task<int> SaveApproveStageRolesAsync(ApproveStageRoleDto role);
        Task<bool> RemoveApproveStageRolesAsync(int roleId);
        Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId);
        #endregion
    }
}
