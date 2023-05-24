using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.BusinessUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAnalysisDimensionRepository
    {
        Task<List<AnalysisDimension>> ByAnalysisDimensionId(int analysisDimensionId, int userId);
        Task<List<BuAnalysisDimension>> ByBusinessUnitId(int businessUnitId, int userId);
        Task<bool> Save(AnalysisDimensionDto analysisDimension, int userId);
        Task<bool> Delete(int analysisDimensionId, int userId);
    }
}
