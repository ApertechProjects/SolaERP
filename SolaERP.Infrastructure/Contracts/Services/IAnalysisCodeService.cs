using SolaERP.Infrastructure.Dtos.AnalysisCode;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IAnalysisCodeService
    {
        public Task<ApiResponse<List<IGrouping<int, AnalysisCodeDto>>>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest);
    }
}
