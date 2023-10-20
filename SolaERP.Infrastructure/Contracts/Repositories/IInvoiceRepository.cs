using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IInvoiceRepository
    {
        Task<bool> ChangeStatus(int invoiceRegisterId, int sequence, int approveStatus, string comment, int userId);
        Task<List<RegisterAll>> RegisterAll(InvoiceRegisterGetModel model, int userId);
        Task<List<RegisterListByOrder>> RegisterListByOrder(int orderMainId);
        Task<List<RegisterLoadGRN>> RegisterLoadGRN(int invoiceRegisterId);
        Task<RegisterMainLoad> RegisterMainLoad(int invoiceRegisterId);
        Task<bool> RegisterSendToApprove(int invoiceRegisterId, int userId);
        Task<List<RegisterWFA>> RegisterWFA(InvoiceRegisterGetModel model, int userId);
    }
}
