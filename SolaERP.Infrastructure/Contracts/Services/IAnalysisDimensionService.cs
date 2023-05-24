using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAnalysisDimensionService
    {
        Task<ApiResponse<List<AnalysisDimensionDto>>> ByAnalysisDimensionId(int analysisDimensionId,string name);
    }
}
