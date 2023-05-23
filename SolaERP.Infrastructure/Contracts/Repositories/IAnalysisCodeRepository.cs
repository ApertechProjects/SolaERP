using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAnalysisCodeRepository
    {
        Task<bool> DeleteAnalysisCodeAsync(int analysisCodeId);
        public Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName);
        Task<List<Analysis>> GetAnalysisCodesAsync(int analysisCodeId, int userId);
        Task<List<AnalysisCodes>> GetAnalysisCodesByDimensionIdAsync(int dimensionId);
        Task<List<AnalysisDimension>> GetAnalysisDimensionAsync();
        Task<List<BuAnalysisDimension>> GetBusinessUnitDimensions(int businessUnitId);
        Task<bool> SaveAnalysisCode(AnalysisDto analysisDto, int userId);
    }
}
