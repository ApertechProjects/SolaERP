using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Order;

namespace SolaERP.Application.Contracts.Services;

public interface IOrderService
{
    Task<ApiResponse<List<OrderTypeLoadDto>>> GetTypesAsync(int businessUnitId);
    Task<ApiResponse<List<OrderAllDto>>> GetAllAsync(OrderFilterDto orderFilterDto, string identityName);
}