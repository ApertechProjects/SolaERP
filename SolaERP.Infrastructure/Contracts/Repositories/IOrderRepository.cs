using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.Order;
using System.Data;

namespace SolaERP.Application.Contracts.Repositories;

public interface IOrderRepository
{
    Task<List<OrderTypeLoadDto>> GetAllOrderTypesByBusinessIdAsync(int businessUnitId);
    Task<List<OrderTab>> GetAllAsync(OrderFilterDto dto, int userId);
    Task<List<OrderTab>> GetWFAAsync(OrderWFAFilterDto filterDto, int userId);
    Task<List<OrderTab>> GetChangeApprovalAsync(OrderChangeApprovalFilterDto filterDto, int userId);
    Task<List<OrderTab>> GetHeldAsync(OrderHeldFilterDto filterDto, int userId);
    Task<List<OrderTab>> GetRejectedAsync(OrderRejectedFilterDto filterDto, int userId);
    Task<List<OrderFilteredDto>> GetDraftAsync(OrderDraftFilterDto filterDto, int userId);
    Task<OrderIUDResponse> SaveOrderMainAsync(OrderMainDto orderMainDto, int userId);
    Task<bool> SaveOrderDetailsAsync(List<OrderDetailDto> orderDetails);

    Task<bool> ChangeOrderMainStatusAsync(ChangeOrderMainStatusDto statusDto, int userId, int orderMainId,
        int sequence);

    Task<bool> SendToApproveAsync(List<int> orderMainIdList, int userId);
    Task<OrderHeadLoaderDto> GetHeaderLoadAsync(int orderMainId);
    Task<List<OrderIntegrateCheckDto>> GetOrderNos(List<int> orderMainIds);
    Task<List<OrderCreateRequestListDto>> GetOrderCreateListForRequestAsync(OrderCreateListRequest dto);
    Task<List<OrderCreateBidListDto>> GetOrderCreateListForBidsAsync(OrderCreateListRequest dto);
    Task<List<OrderDetailLoadDto>> GetAllDetailsAsync(int orderMainId);
    Task<bool> CreateOrderIntegration(int businessUnitId, int orderMainId, int userId);
    Task<int> GetAllIntegratedData(List<OrderIntegrateCheckDto> orders);
    Task<bool> Retrieve(int id, int userId);
    Task<List<AnalysisCodeIds>> GetAnalysis(int businessUnitId, DataTable data);
    Task<List<OrderMainBaseReportInfo>> GetOrderMainBaseReportInfos(List<int> orderMainIds);
    Task<List<int>> GetDetailIds(int orderMainId);
    Task DeleteDetailsNotIncludes(List<int> orderDetailIdList, int orderMaindId);
    Task<bool> CheckOrderList(int bidMainId);
}