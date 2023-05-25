using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveRoleService : ICrudService<ApproveRoleDto>
    {
        Task<ApiResponse<List<ApproveRoleDto>>> ApproveRoleAsync(int businessUnitId);
        Task<ApiResponse<bool>> ApproveRoleDeleteAsync(int roleId, string userName);
        Task<ApiResponse<bool>> ApproveRoleSaveAsync(List<ApproveRoleSaveModel> approveRole, string userName);
    }
}
