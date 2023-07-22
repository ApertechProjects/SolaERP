using SolaERP.Application.Entities.ApproveStage;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IApproveStageRoleRepository : IReturnableAddAsync<ApprovalStageRole>
    {
        Task<List<ApprovalStageRole>> GetByDetailIdAsync(int detailId);
    }
}
