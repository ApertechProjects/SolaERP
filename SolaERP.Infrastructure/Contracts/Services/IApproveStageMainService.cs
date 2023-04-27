using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveStageMainService : ICrudService<ApproveStagesMainDto>, ILoggableCrudService<ApproveStagesMainDto>
    {
        Task<ApiResponse<ApproveStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approveStageMainId);
        Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId);
    }
}
