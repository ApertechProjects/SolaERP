using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAnalysisCodeRepository
    {
        public Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName);
        Task<List<AnalysisCodes>> GetAnalysisCodesByDimensionIdAsync(int dimensionId);
        Task<List<AnalysisDimension>> GetAnalysisDimensionAsync();
        Task<List<BuAnalysisDimension>> GetBusinessUnitDimensions(int businessUnitId);
    }
}
