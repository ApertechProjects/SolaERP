using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Entities.ApproveRole;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IApproveRoleRepository : ICrudOperations<ApproveRole>
    {
        Task<bool> ApproveRoleSaveAsync(ApproveRole model);
        Task<bool> DeleteApproveRoleAsync(int approveRoleId);
    }
}
