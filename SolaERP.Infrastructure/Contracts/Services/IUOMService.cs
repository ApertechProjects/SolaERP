using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UOM;

namespace SolaERP.Application.Contracts.Services
{
    public interface IUOMService : ICrudService<UOMDto>
    {
        Task<ApiResponse<List<UOMDto>>> GetUOMListBusinessUnitCode(int businessUnitId);
    }
}
