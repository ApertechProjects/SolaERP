using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.UOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IUOMService : ICrudService<UOMDto>
    {
        Task<ApiResponse<List<UOMDto>>> GetUOMListBusinessUnitCode(string businessUnitCode);
    }
}
