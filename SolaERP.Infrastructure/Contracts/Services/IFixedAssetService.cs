using SolaERP.Application.Dtos;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services;

public interface IFixedAssetService
{
    Task<ApiResponse<List<FixedAssetDto>>> GetAllAsync(int businessUnitId);
}