using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface INewAnalysisStructureRepository
    {
        Task<List<AnalysisStructureWithBu>> GetByBUAsync(int buId,int procedureId,int userId);
        Task<bool> SaveAsync(AnalysisStructureSaveModel model);
        Task<bool> DeleteAsync(int id, int userId);
        Task<bool> CheckDimensionIdIsUsed(int dimensionId);
    }
}
