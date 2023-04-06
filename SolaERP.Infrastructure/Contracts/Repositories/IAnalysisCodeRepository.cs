using SolaERP.Infrastructure.Dtos.AnaysisDimension;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.AnalysisDimension;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IAnalysisCodeRepository
    {
        Task<bool> DeleteAnalysisCodeAsync(int groupAnalysisCodeId);
        public Task<List<AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName);
        Task<List<GroupAnalysisCode>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<List<AnalysisDimension>> GetAnalysisDimensionAsync();
        Task<bool> SaveAnalysisCodeAsync(AnalysisCodeSaveModel model);
    }
}
