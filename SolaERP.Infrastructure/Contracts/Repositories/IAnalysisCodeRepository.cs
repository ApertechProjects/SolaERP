using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.AnalysisDimension;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IAnalysisCodeRepository
    {
        public Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName);
        Task<List<AnalysisDimension>> GetAnalysisDimensionAsync();
        Task<List<BuAnalysisDimension>> GetBusinessUnitDimensions(int businessUnitId);
    }
}
