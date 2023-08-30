using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Order;

namespace SolaERP.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ApiResponse<List<OrderTypeLoadDto>>> GetTypesAsync(int businessUnitId)
    {
        return ApiResponse<List<OrderTypeLoadDto>>.Success(
            await _orderRepository.GetAllOrderTypesByBusinessIdAsync(businessUnitId)
        );
    }

    public async Task<ApiResponse<List<OrderAllDto>>> GetAllAsync(OrderFilterDto filterDto, string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        return ApiResponse<List<OrderAllDto>>.Success(
            await _orderRepository.GetAllAsync(filterDto, userId)
        );
    }

    public async Task<ApiResponse<List<OrderAllDto>>> GetChangeApprovalAsync(OrderChangeApprovalFilterDto filterDto, 
        string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        return ApiResponse<List<OrderAllDto>>.Success(
            await _orderRepository.GetChangeApprovalAsync(filterDto, userId)
        );
    }
}