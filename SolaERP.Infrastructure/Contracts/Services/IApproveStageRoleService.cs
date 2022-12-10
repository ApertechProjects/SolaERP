using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageRoleService : ICrudService<ApproveStageRoleDto>, ILoggableCrudService<ApproveStageRoleDto>
    {
        Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId);
    }
}
