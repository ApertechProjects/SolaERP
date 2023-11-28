using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.Order;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;

namespace SolaERP.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISupplierEvaluationRepository _supplierRepository;
    private readonly IGeneralService _generalService;
    private readonly IVendorRepository _vendorRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IVendorService _vendorService;
    private readonly IUserRepository _userRepository;

    public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork,
        ISupplierEvaluationRepository supplierRepository, IGeneralService generalService,
        IVendorRepository vendorRepository, IAttachmentService attachmentService, IVendorService vendorService,
        IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _supplierRepository = supplierRepository;
        _generalService = generalService;
        _vendorRepository = vendorRepository;
        _attachmentService = attachmentService;
        _vendorService = vendorService;
        _userRepository = userRepository;
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

        var orderIdList = orderMainDto.OrderDetails.Select(x => x.OrderDetailid).ToList();
        await _orderRepository.DeleteDetailsNotIncludes(orderIdList, mainDto.OrderMainId);

        await _attachmentService.SaveAttachmentAsync( orderMainDto.Attachments,SourceType.ORDER,mainDto.OrderMainId);

        if (orderMainDto.OrderDetails.Count > 0)
        {
            foreach (var detail in orderMainDto.OrderDetails)
            {
                detail.OrderMainId = mainDto.OrderMainId;
                if (detail.RequestDetailId <= 0)
                {
                    detail.RequestDetailId = null;
                }
            }

            var result = await _orderRepository.SaveOrderDetailsAsync(orderMainDto.OrderDetails);

            if (result)
            {
                var detailIds = await _orderRepository.GetDetailIds(mainDto.OrderMainId);
                mainDto.OrderDetailIds = detailIds;
            }
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
        await _orderRepository.ChangeOrderMainStatusAsync(statusDto, userId, statusDto.OrderMainId, statusDto.Sequence);
        var order = await _orderRepository.GetHeaderLoadAsync(statusDto.OrderMainId);
        if (order.ApproveStatus == 1)
        {
            await _vendorService.TransferToIntegration(new CreateVendorRequest
            {
                VendorCode = order.VendorCode,
                UserId = userId,
                BusinessUnitId = order.BusinessUnitId
            });
            await _orderRepository.CreateOrderIntegration(order.BusinessUnitId, statusDto.OrderMainId, userId);
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

    public async Task<ApiResponse<OrderHeadLoaderDto>> GetHeaderLoadAsync(int orderMainId)
    {
        var orderHeader = await _orderRepository.GetHeaderLoadAsync(orderMainId);
        orderHeader.OrderDetails = await _orderRepository.GetAllDetailsAsync(orderMainId);
        orderHeader.Attachments =
            await _attachmentService.GetAttachmentsAsync(orderMainId, SourceType.ORDER, Modules.Orders);
        return ApiResponse<OrderHeadLoaderDto>.Success(orderHeader);
    }

    public async Task<ApiResponse<List<OrderCreateRequestListDto>>> GetOrderCreateListForRequestAsync(
        OrderCreateListRequest dto)
    {
        var businessCategories = (await _generalService.BusinessCategories()).Data;
        var result = await _orderRepository.GetOrderCreateListForRequestAsync(dto);
        for (int i = 0; i < result.Count; i++)
        {
            result[i].BusinessCategoryName =
                businessCategories.SingleOrDefault(y => y.Id == result[i].BusinessCategoryId)?.Name;
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

    public async Task<ApiResponse<WithHoldingTaxData>> WithHoldingTaxDatas(int vendorId)
    {
        var vendor = await _vendorRepository.GetHeader(vendorId);
        var holdingTaxDatas = await _supplierRepository.WithHoldingTaxDatas();

        return ApiResponse<WithHoldingTaxData>.Success(
            holdingTaxDatas.SingleOrDefault(x => x.WithHoldingTaxId == vendor.WithHoldingTaxId)
        );
    }

    public async Task<ApiResponse<bool>> Retrieve(List<int> ids, string name)
    {
        int userId = await _userRepository.ConvertIdentity(name);
        int count = 0;
        string errorIds = string.Empty;
        for (int i = 0; i < ids.Count; i++)
        {
            var result = await _orderRepository.Retrieve(ids[i], userId);
            if (result)
                count++;
            else
                errorIds += ids[i] + ",";
        }

        if (count == ids.Count)
            return ApiResponse<bool>.Success(true);

        else
        {
            errorIds.TrimEnd(',');
            return ApiResponse<bool>.Fail($"{errorIds} orders can not be retrieved", 400);
        }
    }

    public async Task<ApiResponse<List<AnalysisCodeIds>>> GetAnalysis(GetAnalysisByCode model)
    {
        var data = model.AnalysisCodes.ConvertListOfCLassToDataTable();
        var result = await _orderRepository.GetAnalysis(model.BusinessUnitId, data);
        return ApiResponse<List<AnalysisCodeIds>>.Success(result);
    }
}