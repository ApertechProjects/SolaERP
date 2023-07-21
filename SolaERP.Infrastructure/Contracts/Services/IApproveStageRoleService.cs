using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveStageRoleService :
        ICrudService<ApprovalStageRoleDto>,
        ILoggableCrudService<ApprovalStageRoleDto>
    //IReturnableAddAsync<ApproveStageRoleDto>
    {
        Task<ApiResponse<List<ApprovalStageRoleDto>>> GetRoleAsync(int approveStageDetailsId);
    }
}
