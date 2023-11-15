using SolaERP.Application.Dtos.Location;
using SolaERP.Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface ILocationService : ICrudService<LocationDto>
    {
        Task<ApiResponse<List<LocationDto>>> GetAllByBusinessUnitId(int businessUnitId);
    }
}