using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IApproveStageDetailRepository
    {
        Task<List<ApproveStagesDetail>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
        Task<bool> RemoveAsync(int id);
        Task<int> SaveDetailsAsync(ApproveStageDetailInputModel details);
    }
}
