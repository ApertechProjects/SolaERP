using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageMainService : ICrudService<ApproveStagesMainDto>, ILogableCrudService<ApproveStagesMainDto>
    {
        Task<ApiResponse<ApproveStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approveStageMainId);
        Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId);
    }
}
