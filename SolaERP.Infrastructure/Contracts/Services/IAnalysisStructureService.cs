using SolaERP.Application.Dtos.AnalysisStructure;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisStructureService
    {
        Task<ApiResponse<List<AnalysisStructureWithBuDto>>> GetByBUAsync(int buId, int procedureId, string userName);
        Task<ApiResponse<bool>> SaveAsync(List<AnalysisStructureDto> model, string name);
        Task<ApiResponse<bool>> DeleteAsync(AnalysisStructureDeleteModel model, string userName);
      
    }
}
