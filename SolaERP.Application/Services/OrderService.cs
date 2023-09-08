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
    private readonly ISupplierEvaluationRepository _supplierRepository;
    private readonly IGeneralService _generalService;

    public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork,
        ISupplierEvaluationRepository supplierRepository, IGeneralService generalService)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _supplierRepository = supplierRepository;
        _generalService = generalService;
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

    public async Task<ApiResponse<List<OrderFilteredDto>>> GetDraftAsync(OrderDraftFilterDto filterDto,
        string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        return ApiResponse<List<OrderFilteredDto>>.Success(
            await _orderRepository.GetDraftAsync(filterDto, userId)
        );
    }

    public async Task<ApiResponse<OrderIUDResponse>> AddAsync(OrderMainDto orderMainDto, string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        var mainDto = await _orderRepository.SaveOrderMainAsync(orderMainDto, userId);

        if (orderMainDto.OrderDetails.Count > 0)
        {
            foreach (var detail in orderMainDto.OrderDetails)
                detail.OrderMainId = mainDto.OrderMainId;

            await _orderRepository.SaveOrderDetailsAsync(orderMainDto.OrderDetails);
        }

        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<OrderIUDResponse>.Success(mainDto);
    }

    public async Task<ApiResponse<bool>> DeleteAsync(List<int> orderMainIdList, string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        foreach (var orderMainId in orderMainIdList)
        {
            await _orderRepository.SaveOrderMainAsync(new OrderMainDto()
            {
                OrderMainId = orderMainId,
                UserId = userId
            }, userId);
        }

        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true);
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

        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.Success(true);
    }

    public async Task<ApiResponse<bool>> SendToApproveAsync(List<int> orderMainIdList, string identityName)
    {
        int userId = Convert.ToInt32(identityName);
        await _orderRepository.SendToApproveAsync(orderMainIdList, userId);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true);
    }

    public async Task<ApiResponse<List<OrderHeadLoaderDto>>> GetHeaderLoadAsync(int orderMainId)
    {
        return ApiResponse<List<OrderHeadLoaderDto>>.Success(
            await _orderRepository.GetHeaderLoadAsync(orderMainId)
        );
    }

    public async Task<ApiResponse<List<OrderCreateRequestListDto>>> GetOrderCreateListForRequestAsync(
        OrderCreateListRequest dto)
    {
        var businessCategories = (await _generalService.BusinessCategories()).Data;
        var result = await _orderRepository.GetOrderCreateListForRequestAsync(dto);
        for (int i = 0; i < result.Count; i++)
        {
            result[i].BusinessCategoryName = businessCategories.SingleOrDefault(y => y.Id ==  result[i].BusinessCategoryId)?.Name;
            result[i].LineNo = i + 1;
        }
        
        return ApiResponse<List<OrderCreateRequestListDto>>.Success(result);
    }

    public async Task<ApiResponse<List<OrderCreateBidListDto>>> GetOrderCreateListForBidsAsync(
        OrderCreateListRequest dto)
    {
        return ApiResponse<List<OrderCreateBidListDto>>.Success(
            await _orderRepository.GetOrderCreateListForBidsAsync(dto)
        );
    }

    public async Task<ApiResponse<OrderMainGetDto>> GetOrderCardAsync()
    {
        return ApiResponse<OrderMainGetDto>.Success(new OrderMainGetDto
        {
            Countries = await _supplierRepository.GetCountriesAsync(),
            Currencies = await _supplierRepository.GetCurrenciesAsync(),
            DeliveryTerms = await _supplierRepository.GetDeliveryTermsAsync(),
            PaymentTerms = await _supplierRepository.GetPaymentTermsAsync(),
            RejectReasons = (await _generalService.RejectReasons()).Data,
            TaxDatas = await _supplierRepository.TaxDatas()
        });
    }
}