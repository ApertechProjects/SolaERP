using SolaERP.Application.Entities.ApproveStage;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IApproveStageRoleRepository : IReturnableAddAsync<ApproveStageRole>
    {
        Task<List<ApproveStageRole>> GetByDetailIdAsync(int detailId);
    }
}
