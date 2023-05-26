using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisDimensionService
    {
        Task<ApiResponse<List<AnalysisDimensionDto>>> ByAnalysisDimensionId(int analysisDimensionId, string name);
        Task<ApiResponse<List<BuAnalysisDimensionDto>>> ByBusinessUnitId(int businessUnitId, string name);
        Task<ApiResponse<bool>> Save(List<AnalysisDimensionDto> analysisDimension, string name);
        Task<ApiResponse<bool>> Delete(AnalysisDimensionDeleteModel model, string name);
    }
}
