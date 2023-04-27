using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveRoleService : ICrudService<ApproveRoleDto>
    {
        Task<ApiResponse<bool>> ApproveRoleSaveAsync(ApproveRoleDto approveRole);
    }
}
