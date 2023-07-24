using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisCodeService
    {
        Task<ApiResponse<bool>> DeleteAnalysisCodeAsync(AnalysisCodeDeleteModel model, string name);
        public Task<ApiResponse<List<AnalysisCodeModel>>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest);
        //public Task<ApiResponse<List<IGrouping<int,AnalysisCodeModel>>>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest);
        public Task<ApiResponse<List<AnalysisListDto>>> GetAnalysisCodeListAsync(int dimensionId, string userName);
        public Task<ApiResponse<List<AnalysisDto>>> GetAnalysisCodesAsync(int dimensionId, string userName);
        Task<ApiResponse<List<AnalysisWithBuDto>>> GetByBUIdAsync(int businessUnitId, string userName);
        Task<ApiResponse<List<AnalysisCodesDto>>> GetByDimensionIdAsync(int dimensionId);
        Task<ApiResponse<bool>> SaveAnalysisCodeAsync(List<AnalysisCodeSaveModel> analysisCodeSave, string name);
    }
}
