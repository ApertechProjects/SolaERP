using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class AnalysisDimensionService : IAnalysisDimensionService
    {
        public Task<ApiResponse<List<AnalysisDimensionDto>>> GetAnalysisDimensionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
