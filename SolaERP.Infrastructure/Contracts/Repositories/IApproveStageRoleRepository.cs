using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveStageRoleRepository : ICrudOperations<ApproveStageRole>, ILoggableCrudOperations<ApproveStageRole>
    {
        Task<List<ApproveStageRole>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailId);
    }
}
