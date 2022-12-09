using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageDetailService : ICrudService<ApproveStagesDetailDto>
    {
        Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
    }
}
