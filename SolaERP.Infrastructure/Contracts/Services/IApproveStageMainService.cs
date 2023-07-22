using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveStageMainService : ICrudService<ApprovalStagesMainDto>, ILoggableCrudService<ApprovalStagesMainDto>
    {
        Task<ApiResponse<ApprovalStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approveStageMainId);
        Task<ApiResponse<List<ApprovalStagesMainDto>>> GetByBusinessUnitId(int buId);
    }
}
