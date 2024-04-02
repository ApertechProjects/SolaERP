using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.Order;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services;

public interface IOrderService
{
    Task<ApiResponse<List<OrderTypeLoadDto>>> GetTypesAsync(int businessUnitId);
    Task<ApiResponse<List<OrderAllDto>>> GetAllAsync(OrderFilterDto filterDto, string identityName);
    Task<ApiResponse<List<OrderAllDto>>> GetWFAAsync(OrderWFAFilterDto filterDto, string identityName);

    Task<ApiResponse<List<OrderAllDto>>> GetChangeApprovalAsync(OrderChangeApprovalFilterDto filterDto,
        string identityName);

    Task<ApiResponse<List<OrderAllDto>>> GetHeldAsync(OrderHeldFilterDto filterDto, string identityName);
    Task<ApiResponse<List<OrderAllDto>>> GetRejectedAsync(OrderRejectedFilterDto filterDto, string identityName);
    Task<ApiResponse<List<OrderFilteredDto>>> GetDraftAsync(OrderDraftFilterDto filterDto, string identityName);
    Task<ApiResponse<OrderIUDResponse>> AddAsync(OrderMainDto orderMainDto, string identityName);
    Task<ApiResponse<bool>> DeleteAsync(List<int> orderMainIdList, string identityName);
    Task<ApiResponse<bool>> ChangeOrderMainStatusAsync(ChangeOrderMainStatusDto statusDto, string identityName);
    Task<ApiResponse<List<string>>> SendToApproveAsync(List<int> orderMainIdList, string identityName);
    Task<List<OrderMainBaseReportInfoDto>> GetOrderMainBaseReportInfo(List<int> orderMainIds);
    Task<ApiResponse<OrderHeadLoaderDto>> GetHeaderLoadAsync(int orderMainId);
    Task<ApiResponse<List<OrderCreateRequestListDto>>> GetOrderCreateListForRequestAsync(OrderCreateListRequest dto);
    Task<ApiResponse<List<OrderCreateBidListDto>>> GetOrderCreateListForBidsAsync(OrderCreateListRequest dto);
    Task<ApiResponse<OrderMainGetDto>> GetOrderCardAsync();
    Task<ApiResponse<WithHoldingTaxData>> WithHoldingTaxDatas(int vendorId);
    Task<ApiResponse<bool>> Retrieve(List<int> ids, string name);
    Task<ApiResponse<List<AnalysisCodeIds>>> GetAnalysis(GetAnalysisByCode model);
}