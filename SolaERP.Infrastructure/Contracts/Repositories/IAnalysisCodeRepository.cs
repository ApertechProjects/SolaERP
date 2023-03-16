using SolaERP.Infrastructure.Entities.AnalysisCode;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IAnalysisCodeRepository
    {
        public Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName);
    }
}
