using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveRoleService : ICrudService<ApproveRoleDto>
    {
        Task<ApiResponse<bool>> ApproveRoleSaveAsync(ApproveRoleDto approveRole);
    }
}
