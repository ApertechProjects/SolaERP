using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services;

public interface IOrderService
{
    Task<ApiResponse<string>> GetTypesAsync(int businessUnitId);
}