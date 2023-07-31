using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Models;
using AnalysisCodes = SolaERP.Application.Entities.AnalysisCode.AnalysisCodes;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAnalysisStructureRepository
    {
        Task<bool> DeleteAnalysisCodeAsync(int analysisCodeId, int userId);
        public Task<List<Entities.AnalysisCode.AnalysisCode>> GetAnalysisCodesAsync(int businessUnitId, string procedureName);
        Task<List<Analysis>> GetAnalysisCodesAsync(int analysisCodeId, int userId);
        Task<List<AnalysisWithBu>> GetByBUIdAsync(int businessUnitId, int userId);
        Task<List<AnalysisCodes>> GetAnalysisCodesByDimensionIdAsync(int dimensionId);
        Task<bool> SaveAnalysisCode(AnalysisCodeSaveModel analysisCodeSave, int userId);
    }
}
