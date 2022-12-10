using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveStageDetailRepository : ICrudOperations<ApproveStagesDetail>, ILoggableCrudOperations<ApproveStagesDetail>
    {
        Task<List<ApproveStagesDetail>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
    }
}
