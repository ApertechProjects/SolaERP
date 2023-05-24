using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisCodeService
    {
        Task<ApiResponse<bool>> DeleteAnalysisCodeAsync(int analysisCodeId, string name);
        public Task<ApiResponse<List<IGrouping<int, AnalysisCodeDto>>>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest);
        public Task<ApiResponse<List<AnalysisDto>>> GetAnalysisCodesAsync(int analysisCodeId, string userName);
        Task<ApiResponse<List<AnalysisWithBuDto>>> GetByBUIdAsync(int businessUnitId, string userName);
        Task<ApiResponse<List<AnalysisCodesDto>>> GetByDimensionIdAsync(int dimensionId);
        Task<ApiResponse<bool>> SaveAnalysisCodeAsync(AnalysisCodeSaveModel analysisCodeSave, string name);
    }
}
