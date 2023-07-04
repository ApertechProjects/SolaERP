using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IApproveStageDetailService
    {
        Task<ApiResponse<List<ApproveStagesDetailDto>>> GetDetailByIdAsync(int approveStageMainId);
    }
}
