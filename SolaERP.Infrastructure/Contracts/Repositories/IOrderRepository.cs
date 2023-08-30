using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Entities.Order;

namespace SolaERP.Application.Contracts.Repositories;

public interface IOrderRepository
{
    Task<List<OrderTypeLoadDto>> GetAllOrderTypesByBusinessIdAsync(int businessUnitId);
    Task<List<OrderAllDto>> GetAllAsync(OrderFilterDto dto, int userId);

    Task<List<OrderAllDto>> GetChangeApprovalAsync(OrderChangeApprovalFilterDto filterDto, int userId);
}