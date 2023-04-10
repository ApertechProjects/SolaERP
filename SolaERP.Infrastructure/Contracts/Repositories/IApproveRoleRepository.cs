using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Entities.ApproveRole;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveRoleRepository : ICrudOperations<ApproveRole>
    {
        Task<bool> ApproveRoleSaveAsync(ApproveRole model);
        Task<bool> DeleteApproveRoleAsync(int approveRoleId);
    }
}
