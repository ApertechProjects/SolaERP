using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveStageRoleRepository : IReturnableAddAsync<ApproveStageRole>
    {
        Task<List<ApproveStageRole>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailId);
    }
}
