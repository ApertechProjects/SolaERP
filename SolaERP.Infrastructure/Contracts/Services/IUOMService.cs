using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IUOMService : ICrudService<UOMDto>
    {
        Task<ApiResponse<List<UOMDto>>> GetUOMListBusinessUnitCode(string businessUnitCode);
    }
}
