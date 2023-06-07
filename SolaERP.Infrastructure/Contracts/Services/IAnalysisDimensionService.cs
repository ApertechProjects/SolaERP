using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisDimensionService
    {
        Task<ApiResponse<List<AnalysisDimensionDto>>> ByDimensionIdAsync(int analysisDimensionId, string name);
        Task<ApiResponse<List<BuAnalysisDimensionDto>>> ByBUIdAsync(int businessUnitId, string name);
        Task<ApiResponse<bool>> SaveAsync(List<AnalysisDimensionDto> analysisDimension, string name);
        Task<ApiResponse<bool>> DeleteAsync(AnalysisDimensionDeleteModel model, string name);
        Task<List<DimensionCheckDto>> CheckDimensionIdsAsync(List<int> dimensionIds);
    }
}
