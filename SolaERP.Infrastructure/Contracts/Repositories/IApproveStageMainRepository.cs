using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.Entities.ApproveStages;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IApproveStageMainRepository : ILoggableCrudOperations<ApproveStagesMain>
    {
        Task<ApproveStagesMain> GetApprovalStageHeaderLoad(int approvalStageMainId);
        Task<List<ApproveStagesMain>> GetByBusinessUnitId(int buId);
        Task<List<ApprovalStatus>> GetApprovalStatusList();
        Task<bool> DeleteApproveStageAsync(int approveStageMainId);
    }
}
