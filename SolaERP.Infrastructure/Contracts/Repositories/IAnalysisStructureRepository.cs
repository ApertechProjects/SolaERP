using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface INewAnalysisStructureRepository
    {
        Task<AnalysisStructure> GetByIdAsync(int id);
        Task<AnalysisStructureWithBu> GetByBUAsync(int buId,int procedureId,int userId);
        Task<bool> AddAsync(AnalysisStructureSaveModel model);
        Task<bool> UpdateAsync(AnalysisStructureDeleteModel model);
        Task<bool> RemoveAsync(int id, int userId);
    }
}
