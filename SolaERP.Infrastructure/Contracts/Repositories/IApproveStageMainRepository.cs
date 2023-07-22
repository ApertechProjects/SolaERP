using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.Entities.ApproveStages;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IApproveStageMainRepository : ILoggableCrudOperations<ApprovalStagesMain>
    {
        Task<ApprovalStagesMain> GetApprovalStageHeaderLoad(int approvalStageMainId);
        Task<List<ApprovalStagesMain>> GetByBusinessUnitId(int buId);
        Task<List<ApprovalStatus>> GetApprovalStatusList();
        Task<bool> DeleteApproveStageAsync(int approveStageMainId);
        Task<int> SaveApproveStageMainAsync(ApproveStageMainInputModel entity, int userId);
        Task<bool> CheckVendorStage();
        Task<List<ApprovalStages>> Stages(int businessUnitId, string procedureKey);
    }
}
