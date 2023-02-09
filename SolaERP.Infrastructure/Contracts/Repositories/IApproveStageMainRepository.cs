using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.Entities.ApproveStages;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveStageMainRepository : ILoggableCrudOperations<ApproveStagesMain>
    {
        Task<ApproveStagesMain> GetApprovalStageHeaderLoad(int approvalStageMainId);
        Task<List<ApproveStagesMain>> GetByBusinessUnitId(int buId);
        Task<List<ApprovalStatus>> GetApprovalStatusList();
    }
}
