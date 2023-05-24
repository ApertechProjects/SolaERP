using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisService
    {
        Task<AnalysisStructure> GetByIdAsync(int id);
        Task<AnalysisStructureWithBu> GetByBUAsync(int buId);
        Task<bool> AddAsync(AnalysisStructureInputModel model);
        Task<bool> UpdateAsync(AnalysisStructureUpdateModel model);
        Task<bool> RemoveAsync(int id, int userId);
    }
}
