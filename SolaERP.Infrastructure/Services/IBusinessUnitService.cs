using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Services
{
    public interface IBusinessUnitService : ICrudService<BusinessUnitsAllDto>
    {
        Task<ApiResponse<List<BusinessUnitsDto>>> GetBusinessUnitListByUserId(int userId);
    }
}
