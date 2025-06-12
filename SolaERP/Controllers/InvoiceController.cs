using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class InvoiceController : CustomBaseController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }


        [HttpGet]
        public async Task<IActionResult> RegisterWFA([FromQuery] InvoiceRegisterGetModel model)
            => CreateActionResult(await _invoiceService.RegisterWFA(model, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> RegisterAll([FromQuery] InvoiceRegisterGetModel model)
            => CreateActionResult(await _invoiceService.RegisterAll(model, User.Identity.Name));


        [HttpGet]
        public async Task<IActionResult> RegisterDraft([FromQuery] InvoiceRegisterGetModel model)
            => CreateActionResult(await _invoiceService.RegisterDraft(model, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> RegisterHeld([FromQuery] InvoiceRegisterGetModel model)
           => CreateActionResult(await _invoiceService.RegisterHeld(model, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> RegisterSendToApprove(InvoiceSendToApproveModel model)
            => CreateActionResult(await _invoiceService.RegisterSendToApprove(model, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> Info(int invoiceRegisterId)
            => CreateActionResult(await _invoiceService.Info(invoiceRegisterId, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> ApprovalInfo(int invoiceRegisterId)
        => CreateActionResult(await _invoiceService.ApprovalInfos(invoiceRegisterId, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> RegisterLoadGRN(int orderMainId)
            => CreateActionResult(await _invoiceService.RegisterLoadGRN(orderMainId));

        [HttpGet]
        public async Task<IActionResult> RegisterListByOrder(int orderMainId)
            => CreateActionResult(await _invoiceService.RegisterListByOrder(orderMainId));


        [HttpPost]
        public async Task<IActionResult> RegisterChangeStatus(InvoiceRegisterApproveModel model)
            => CreateActionResult(await _invoiceService.RegisterChangeStatus(model, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Save(List<InvoiceRegisterSaveModel> model)
            => CreateActionResult(await _invoiceService.Save(model, User.Identity.Name));


        [HttpDelete]
        public async Task<IActionResult> Delete(List<int> ids)
            => CreateActionResult(await _invoiceService.Delete(ids, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetOrderListApproved(int businessUnitId, string vendorCode)
            => CreateActionResult(await _invoiceService.GetOrderListApproved(businessUnitId, vendorCode));

        [HttpGet]
        public async Task<IActionResult> GetProblematicInvoiceReasonList()
            => CreateActionResult(await _invoiceService.GetProblematicInvoiceReasonList());

        [HttpGet]
        public async Task<IActionResult> MatchingMainGRN([FromQuery] InvoiceMatchingGRNModel model)
            => CreateActionResult(await _invoiceService.MatchingMainGRN(model));

        [HttpGet]
        public async Task<IActionResult> MatchingMainGRNList([FromQuery] InvoiceMatchingMainModel model)
          => CreateActionResult(await _invoiceService.MatchingMainGRNList(model));

        [HttpGet]
        public async Task<IActionResult> MatchingMainService([FromQuery] InvoiceMatchingGRNModel model)
            => CreateActionResult(await _invoiceService.MatchingMainService(model));

        [HttpGet]
        public async Task<IActionResult> GetMatchingMain([FromQuery] int orderMainId)
            => CreateActionResult(await _invoiceService.GetMatchingMain(orderMainId));

        [HttpPost]
        public async Task<IActionResult> GetDetails(InvoiceGetDetailsModel model)
        {
            string keyCode = await _invoiceService.GetKeyCode(model.OrderMainId);
            if (keyCode == "PO")
                return CreateActionResult(await _invoiceService.GetDetailsForPO(model));

            return CreateActionResult(await _invoiceService.GetDetailsForOtherOrderTypes(model));
        }

        [HttpGet("{businessUnitId:int}")]
        public async Task<IActionResult> GetTransactionReferenceList(int businessUnitId)
            => CreateActionResult(await _invoiceService.GetTransactionReferenceList(businessUnitId));

        [HttpGet("{businessUnitId:int}")]
        public async Task<IActionResult> GetReferenceList(int businessUnitId)
            => CreateActionResult(await _invoiceService.GetReferenceList(businessUnitId));

        [HttpGet("{businessUnitId:int}")]
        public async Task<IActionResult> GetInvoiceList(int businessUnitId)
            => CreateActionResult(await _invoiceService.GetInvoiceList(businessUnitId));

        [HttpGet("{orderMainId:int}")]
        public async Task<IActionResult> GetAdvanceInvoicesList(int orderMainId)
            => CreateActionResult(await _invoiceService.GetAdvanceInvoicesList(orderMainId));

        [HttpPost]
        public async Task<IActionResult> SaveInvoiceMatchingForService(SaveInvoiceMatchingModel model)
            => CreateActionResult(await _invoiceService.SaveInvoiceMatchingForService(model, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> SaveInvoiceMatchingForGRN(SaveInvoiceMatchingGRNModel model)
           => CreateActionResult(await _invoiceService.SaveInvoiceMatchingForGRN(model, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> SaveInvoiceMatchingGRNs(InvoiceMatchingGRNs request)
            => CreateActionResult(await _invoiceService.SaveInvoiceMatchingGRNs(request));

        [HttpGet("{orderMainId}")]
        public async Task<IActionResult> InvoiceRegisterList(int orderMainId)
            => CreateActionResult(await _invoiceService.InvoiceRegisterList(orderMainId));

        [HttpPost]
        public async Task<IActionResult> InvoiceRegisterServiceDetailsLoad(InvoiceRegisterServiceLoadModel model)
            => CreateActionResult(await _invoiceService.InvoiceRegisterServiceDetailsLoad(model));

        [HttpGet("{invoiceMatchingId}/{businessUnitId}")]
        public async Task<IActionResult> GetInvoiceMatchData(int invoiceMatchingId, int businessUnitId)
            => CreateActionResult(await _invoiceService.GetInvoiceMatchData(invoiceMatchingId, businessUnitId));


        [HttpGet]
        public async Task<IActionResult> InvoiceRegisterInfo(int invoiceRegisterId)
           => CreateActionResult(await _invoiceService.GetInvoiceRegisterLoad(invoiceRegisterId, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetInvoiceRegisterPayablesTransactions(int invoiceRegisterId)
          => CreateActionResult(await _invoiceService.GetInvoiceRegisterPayablesTransactions(invoiceRegisterId));

		[HttpGet("{businessUnitId}")]
		public async Task<IActionResult> GetPeriodList(int businessUnitId)
			=> CreateActionResult(await _invoiceService.GetPeriodList(businessUnitId));

		[HttpGet("{orderMainId}")]
		public async Task<IActionResult> GetRegisterOrderDetails(int orderMainId)
			=> CreateActionResult(await _invoiceService.GetRegisterOrderDetails(orderMainId));
        
        [HttpGet]
        public async Task<IActionResult> InvoiceRegisterAdvance(int businessUnitId, DateTime dateFrom, DateTime dateTo)
            => CreateActionResult(await _invoiceService.GetInvoiceRegisterAdvance(businessUnitId, dateFrom, dateTo, Convert.ToInt32(User.Identity.Name)));
        
        [HttpGet]
        public async Task<IActionResult> InvoiceRegisterAdvanceClosingList(int invoiceRegisterId)
            => CreateActionResult(await _invoiceService.GetInvoiceRegisterAdvanceClosingList(invoiceRegisterId));
        
        [HttpGet]
        public async Task<IActionResult> InvoiceRegisterInvoiceDetailsForCreditNote(int invoiceRegisterId)
            => CreateActionResult(await _invoiceService.GetInvoiceRegisterInvoiceDetailsForCreditNote(invoiceRegisterId));
        
        [HttpPost]
        public async Task<IActionResult> SaveAdvanceClosing(List<InvoiceClosingRequest> model)
            => CreateActionResult(await _invoiceService.SaveAdvanceClosing(model, Convert.ToInt32(User.Identity.Name)));
        
        [HttpGet]
        public async Task<IActionResult> GetInvoiceRegisterList(int businessUnitId)
            => CreateActionResult(await _invoiceService.GetInvoiceRegisterList(businessUnitId));
    }
}