using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IApproveRoleRepository : ICrudOperations<ApproveRole>
    {
        Task<List<ApproveRole>> ApproveRoleAsync(int businessUnitId);
        Task<bool> ApproveRoleSaveAsync(ApproveRoleSaveModel model, int userId);
        Task<bool> ApproveRoleDeleteAsync(int approveRoleId,int userId);
    }
}
