using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveStageMainRepository : ICrudOperations<ApproveStagesMain>
    {
        Task<ApproveStagesMain> GetApprovalStageHeaderLoad(int approvalStageMainId);
        Task<List<ApproveStagesMain>> GetByBusinessUnitId(int buId);
    }
}
