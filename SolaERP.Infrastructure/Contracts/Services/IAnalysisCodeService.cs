using SolaERP.Infrastructure.Dtos.AnalysisCode;
using SolaERP.Infrastructure.Dtos.AnaysisDimension;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IAnalysisCodeService
    {
        public Task<ApiResponse<List<IGrouping<int, AnalysisCodeDto>>>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest);
        Task<ApiResponse<List<GroupAnalysisCodeDto>>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<ApiResponse<List<AnalysisDimensionDto>>> GetAnalysisDimensionAsync();
        Task<ApiResponse<bool>> SaveAnalysisCodeAsync(AnalysisCodeSaveModel model);
    }
}
