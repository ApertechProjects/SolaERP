using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageService : ILoggableCrudService<ApproveStagesMainDto>
    {
        #region Main
        Task<ApiResponse<ApproveStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approveStageMainId);
        Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId);
        #endregion
        //
        #region Details
        Task<int> SaveApproveStageDetailsAsync(List<ApproveStagesDetailDto> details);
        Task<int> RemoveApproveStageDetailsAsync(int approveStageDetailsId);
        Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
        #endregion
        //
        #region Roles
        Task<int> SaveApproveStageRolesAsync(List<ApproveStageRoleDto> roles);
        Task<int> RemoveApproveStageolesAsync(int roleId);
        Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId);
        #endregion
    }
}
