using SolaERP.Application.Dtos;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
	public interface IInvoiceService
	{
		Task<ApiResponse<List<RegisterAllDto>>> RegisterAll(InvoiceRegisterGetModel model, string name);
		Task<ApiResponse<bool>> RegisterChangeStatus(InvoiceRegisterApproveModel model, string name);
		Task<ApiResponse<List<RegisterListByOrderDto>>> RegisterListByOrder(int orderMainId);
		Task<ApiResponse<List<RegisterLoadGRNDto>>> RegisterLoadGRN(int orderMainId);
		Task<ApiResponse<RegisterMainLoadDto>> Info(int invoiceRegisterId, string name);
		Task<ApiResponse<bool>> RegisterSendToApprove(InvoiceSendToApproveModel model, string name);
		Task<ApiResponse<List<RegisterWFADto>>> RegisterWFA(InvoiceRegisterGetModel model, string name);
		Task<ApiResponse<bool>> Save(List<InvoiceRegisterSaveModel> model, string name);
		Task<ApiResponse<List<OrderListApprovedDto>>> GetOrderListApproved(int businessUnitId, string vendorCode);
		Task<ApiResponse<List<ProblematicInvoiceReasonDto>>> GetProblematicInvoiceReasonList();
		Task<ApiResponse<bool>> Delete(List<int> ids, string name);
		Task<ApiResponse<List<MatchingMainGRNDto>>> MatchingMainGRN(InvoiceMatchingGRNModel model);
		Task<ApiResponse<List<MatchingMainServiceDto>>> MatchingMainService(InvoiceMatchingGRNModel model);
		Task<ApiResponse<MatchingMainDto>> GetMatchingMain(int orderMainId);
		Task<ApiResponse<List<InvoiceRegisterDetailForPODto>>> GetDetailsForPO(InvoiceGetDetailsModel model);

		Task<ApiResponse<List<InvoiceRegisterDetailForOtherDto>>> GetDetailsForOtherOrderTypes(
			InvoiceGetDetailsModel model);

		Task<string> GetKeyCode(int invoiceRegisterId);
		Task<ApiResponse<List<string>>> GetTransactionReferenceList(int businessUnitId);
		Task<ApiResponse<List<string>>> GetReferenceList(int businessUnitId);
		Task<ApiResponse<List<string>>> GetInvoiceList(int businessUnitId);
		Task<ApiResponse<List<AdvanceInvoiceDto>>> GetAdvanceInvoicesList(int orderMainId);
		Task<ApiResponse<bool>> SaveInvoiceMatchingGRNs(InvoiceMatchingGRNs request);
		Task<ApiResponse<bool>> SaveInvoiceMatchingAdvances(InvoiceMatchingAdvance request);
		Task<ApiResponse<InvoiceRegisterByOrderMainIdDto>> InvoiceRegisterList(int orderMainId);
		Task<ApiResponse<List<InvoiceRegisterServiceDetailsLoadDto>>> InvoiceRegisterServiceDetailsLoad(InvoiceRegisterServiceLoadModel model);
		Task<ApiResponse<SaveResultModel>> SaveInvoiceMatchingForService(SaveInvoiceMatchingModel model, string userName);
		Task<ApiResponse<SaveResultModel>> SaveInvoiceMatchingForGRN(SaveInvoiceMatchingGRNModel model, string userName);

		Task<ApiResponse<bool>> CheckInvoiceRegister(int invocieRegisterId, int businessUnitId, string vendorCode, string invoiceNo);
		Task<ApiResponse<List<RegisterDraftDto>>> RegisterDraft(InvoiceRegisterGetModel model, string name);
		Task<ApiResponse<List<RegisterHeldDto>>> RegisterHeld(InvoiceRegisterGetModel model, string name);
		Task<ApiResponse<List<ApprovalInfoDto>>> ApprovalInfos(int invoiceRegisterId, string name);
		Task<ApiResponse<List<InvoiceMatchingMainGRNDto>>> MatchingMainGRNList(InvoiceMatchingMainModel model);
		Task<ApiResponse<InvoiceMatchResultModelDto>> GetInvoiceMatchData(int invoiceMatchingMainId, int businessUnitId);
		Task<ApiResponse<bool>> InvoiceRegisterDetailsSave(int invoiceRegisterMainId, List<InvoiceRegisterDetails> details);
		Task<ApiResponse<InvoiceRegisterLoadDto>> GetInvoiceRegisterLoad(int invoiceRegisterId);
		Task<ApiResponse<List<InvoiceRegisterPayablesTransactionsDto>>> GetInvoiceRegisterPayablesTransactions(int invoiceRegisterId);
	}
}