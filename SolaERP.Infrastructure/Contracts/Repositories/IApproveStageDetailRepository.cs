using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveStageDetailRepository
    {
        Task<List<ApproveStagesDetail>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
        Task<bool> RemoveAsync(int id);
        Task<int> SaveDetailsAsync(ApproveStagesDetail details);
    }
}
