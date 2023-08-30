using SolaERP.Application.Entities.Order;

namespace SolaERP.Application.Contracts.Repositories;

public interface IOrderRepository
{
    Task<List<OrderTypeLoadDto>> GetAllOrderTypesByBusinessIdAsync(int businessUnitId);
    
    
}