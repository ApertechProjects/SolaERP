using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveStageDetailService : ICrudService<ApproveStagesDetailDto>, ILoggableCrudService<ApproveStagesDetailDto>
    {
        Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
    }
}
