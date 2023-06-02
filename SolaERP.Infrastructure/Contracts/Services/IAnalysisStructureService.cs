using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisStructureService
    {
        Task<AnalysisStructure> GetByIdAsync(int id);
        Task<AnalysisStructureWithBu> GetByBUAsync(int buId,int procedureId,string userName);
        Task<bool> AddAsync(AnalysisStructureSaveModel model);
        Task<bool> UpdateAsync(AnalysisStructureDeleteModel model);
        Task<bool> RemoveAsync(int id, int userId);
    }
}
