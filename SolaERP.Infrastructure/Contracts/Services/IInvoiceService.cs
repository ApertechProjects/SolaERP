using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IInvoiceService
    {
        Task<ApiResponse<List<RegisterAllDto>>> RegisterAll(InvoiceRegisterGetModel model, string name);
        Task<ApiResponse<bool>> RegisterSendToApprove(InvoiceSendToApproveModel model, string name);
        Task<ApiResponse<List<RegisterWFADto>>> RegisterWFA(InvoiceRegisterGetModel model, string name);
    }
}
