using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Order;

namespace SolaERP.Application.Contracts.Services;

public interface IOrderService
{
    Task<ApiResponse<List<OrderTypeLoadDto>>> GetTypesAsync(int businessUnitId);
}