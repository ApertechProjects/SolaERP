using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IInvoiceService
    {
        Task<ApiResponse<List<RegisterAllDto>>> RegisterAll(InvoiceRegisterGetModel model, string name);
        Task<ApiResponse<List<RegisterListByOrderDto>>> RegisterListByOrder(int orderMainId);
        Task<ApiResponse<List<RegisterLoadGRNDto>>> RegisterLoadGRN(int invoiceRegisterId);
        Task<ApiResponse<RegisterMainLoadDto>> RegisterLoadMain(int invoiceRegisterId);
        Task<ApiResponse<bool>> RegisterSendToApprove(InvoiceSendToApproveModel model, string name);
        Task<ApiResponse<List<RegisterWFADto>>> RegisterWFA(InvoiceRegisterGetModel model, string name);
    }
}
