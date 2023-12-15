using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Models;
using System.Data;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IInvoiceRepository
    {
        Task<bool> ChangeStatus(int invoiceRegisterId, int sequence, int approveStatus, string comment, int userId,int? rejectReasonId);
        Task<List<RegisterAll>> RegisterAll(InvoiceRegisterGetModel model, int userId);
        Task<List<RegisterListByOrder>> RegisterListByOrder(int orderMainId);
        Task<List<RegisterLoadGRN>> RegisterLoadGRN(int invoiceRegisterId);
        Task<RegisterMainLoad> RegisterMainLoad(int invoiceRegisterId);
        Task<List<ApprovalInfo>> ApprovalInfos(int invoiceRegisterId, int userId);
        Task<bool> RegisterSendToApprove(int invoiceRegisterId, int userId);
        Task<List<RegisterWFA>> RegisterWFA(InvoiceRegisterGetModel model, int userId);
        Task<int> Save(InvoiceRegisterSaveModel model, int userId);
        Task<List<OrderListApproved>> GetOrderListApproved(int businessUnitId, string vendorCode);
        Task<List<ProblematicInvoiceReason>> GetProblematicInvoiceReasonList();
        Task<bool> Delete(int item, int userId);
        Task<List<MatchingMainGRN>> MatchingMainGRN(InvoiceMatchingGRNModel model);
        Task<List<MatchingMainService>> MatchingMainService(InvoiceMatchingGRNModel model);
        Task<MatchingMain> GetMatchingMain(int orderMainId);
        Task<List<InvoiceRegisterDetailForPO>> GetDetailsForPO(InvoiceGetDetailsModel model);
        Task<List<InvoiceRegisterDetailForOther>> GetDetailsForOther(InvoiceGetDetailsModel model);
        Task<string> GetKeyKode(int invoiceRegisterId);
        Task<List<string>> GetTransactionReferenceList(int businessUnitId);
        Task<List<string>> GetReferenceList(int businessUnitId);
        Task<List<string>> GetInvoiceList(int businessUnitId);
        Task<List<AdvanceInvoice>> GetAdvanceInvoicesList(int orderMainId);
        Task<int> SaveInvoiceMatchingMain(InvoiceMathcingMain request, int userId);
        Task<bool> SaveInvoiceMatchingGRNs(int requestInvoiceMatchingMainId, DataTable dataTable);

        Task<bool> SaveInvoiceMatchingAdvances(int requestInvoiceRegisterId, int requestMatchingId,
            DataTable dataTable);

        Task<bool> SaveInvoiceMatchingDetails(int requestInvoiceMatchingMainid, DataTable dataTable);
        Task<List<int>> GetDetailIds(int invoiceMatchingMainId);
        Task<bool> InvoiceIUD(int businessUnitId, int invoiceRegisterId, int userId);
        Task<InvoiceRegisterByOrderMainId> InvoiceRegisterList(int orderMainId);
        Task<List<InvoiceRegisterServiceDetailsLoad>> InvoiceRegisterDetailsLoad(InvoiceRegisterServiceLoadModel model);
        Task<bool> CheckInvoiceRegister(int invoiceRegisterId, int businessUnit, string vendorCode, string invoiceNo);
        Task<List<RegisterDraft>> RegisterDraft(InvoiceRegisterGetModel model, int userId);
        Task<List<RegisterHeld>> RegisterHeld(InvoiceRegisterGetModel model, int userId);
    }
}