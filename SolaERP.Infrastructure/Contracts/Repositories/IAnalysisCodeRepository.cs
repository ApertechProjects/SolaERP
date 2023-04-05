using SolaERP.Infrastructure.Dtos.AnaysisDimension;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.AnalysisDimension;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IAnalysisCodeRepository
    {
        public Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName);
        Task<List<GroupAnalysisCode>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<List<AnalysisDimension>> GetAnalysisDimensionAsync();
        Task SaveAnalysisCodeAsync(AnalysisCodeSaveModel model);
    }
}
