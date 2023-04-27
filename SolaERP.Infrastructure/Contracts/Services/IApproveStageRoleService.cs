using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveStageRoleService :
        ICrudService<ApproveStageRoleDto>,
        ILoggableCrudService<ApproveStageRoleDto>
    //IReturnableAddAsync<ApproveStageRoleDto>
    {
        Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId);
    }
}
