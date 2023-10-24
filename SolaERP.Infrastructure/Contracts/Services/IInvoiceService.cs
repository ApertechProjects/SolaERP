using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IInvoiceService
    {
        Task<ApiResponse<List<RegisterAllDto>>> RegisterAll(InvoiceRegisterGetModel model, string name);
        Task<ApiResponse<bool>> RegisterChangeStatus(InvoiceRegisterApproveModel model, string name);
        Task<ApiResponse<List<RegisterListByOrderDto>>> RegisterListByOrder(int orderMainId);
        Task<ApiResponse<List<RegisterLoadGRNDto>>> RegisterLoadGRN(int invoiceRegisterId);
        Task<ApiResponse<RegisterMainLoadDto>> RegisterLoadMain(int invoiceRegisterId);
        Task<ApiResponse<bool>> RegisterSendToApprove(InvoiceSendToApproveModel model, string name);
        Task<ApiResponse<List<RegisterWFADto>>> RegisterWFA(InvoiceRegisterGetModel model, string name);
        Task<ApiResponse<bool>> Save(InvoiceRegisterSaveModel model, string name);
        Task<ApiResponse<List<OrderListApprovedDto>>> GetOrderListApproved(int businessUnitId, string vendorCode);
        Task<ApiResponse<List<ProblematicInvoiceReasonDto>>> GetProblematicInvoiceReasonList();
        Task<ApiResponse<bool>> Delete(List<int> ids, string name);
        Task<ApiResponse<List<MatchingMainGRNDto>>> MatchingMainGRN(InvoiceMatchingGRNModel model);
        Task<ApiResponse<List<MatchingMainServiceDto>>> MatchingMainService(InvoiceMatchingGRNModel model);
    }
}