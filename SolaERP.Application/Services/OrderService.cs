using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.Order;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
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

    public async Task<ApiResponse<List<OrderAllDto>>> GetWFAAsync(OrderWFAFilterDto filterDto,
        string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        return ApiResponse<List<OrderAllDto>>.Success(
            await _orderRepository.GetWFAAsync(filterDto, userId)
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

    public async Task<ApiResponse<List<OrderAllDto>>> GetHeldAsync(OrderHeldFilterDto filterDto,
        string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        return ApiResponse<List<OrderAllDto>>.Success(
            await _orderRepository.GetHeldAsync(filterDto, userId)
        );
    }

    public async Task<ApiResponse<List<OrderAllDto>>> GetRejectedAsync(OrderRejectedFilterDto filterDto,
        string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        return ApiResponse<List<OrderAllDto>>.Success(
            await _orderRepository.GetRejectedAsync(filterDto, userId)
        );
    }

    public async Task<ApiResponse<List<OrderAllDto>>> GetDraftAsync(OrderDraftFilterDto filterDto,
        string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        return ApiResponse<List<OrderAllDto>>.Success(
            await _orderRepository.GetDraftAsync(filterDto, userId)
        );
    }

    public async Task<ApiResponse<OrderIUDResponse>> AddAsync(OrderMainDto orderMainDto, string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        var mainDto = await _orderRepository.SaveOrderMainAsync(orderMainDto, userId);

        foreach (var detail in orderMainDto.OrderDetails)
            detail.OrderMainId = mainDto.OrderMainId;

        await _orderRepository.SaveOrderDetailsAsync(orderMainDto.OrderDetails);

        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<OrderIUDResponse>.Success(mainDto);
    }

    public async Task<ApiResponse<OrderIUDResponse>> DeleteAsync(int orderMainId, string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        var mainDto = await _orderRepository.SaveOrderMainAsync(new OrderMainDto()
        {
            OrderMainId = orderMainId,
            UserId = userId
        }, userId);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<OrderIUDResponse>.Success(mainDto);
    }

    public async Task<ApiResponse<bool>> ChangeOrderMainStatusAsync(ChangeOrderMainStatusDto statusDto,
        string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        foreach (var selectedOrder in statusDto.SelectedList)
        {
            await _orderRepository.ChangeOrderMainStatusAsync(statusDto, userId, selectedOrder.OrderMainId,
                selectedOrder.Sequence);
        }

        return ApiResponse<bool>.Success(true);
    }
}