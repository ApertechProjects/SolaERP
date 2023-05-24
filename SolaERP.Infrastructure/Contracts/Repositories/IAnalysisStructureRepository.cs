using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface INewAnalysisStructureRepository
    {
        Task<AnalysisStructure> GetByIdAsync(int id);
        Task<AnalysisStructureWithBu> GetByBUAsync(int buId);
        Task<bool> AddAsync(AnalysisStructureInputModel model);
        Task<bool> UpdateAsync(AnalysisStructureUpdateModel model);
        Task<bool> RemoveAsync(int id, int userId);
    }
}
