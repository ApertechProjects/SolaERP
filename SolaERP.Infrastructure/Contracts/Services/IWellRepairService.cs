using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.WellRepair;

namespace SolaERP.Application.Contracts.Services;

public interface IWellRepairService
{
    Task<ApiResponse<List<WellRepairListDto>>> GetWellRepairList();
}