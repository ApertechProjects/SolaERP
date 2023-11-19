using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
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

        [HttpPost]
        public async Task<IActionResult> RegisterSendToApprove(InvoiceSendToApproveModel model)
            => CreateActionResult(await _invoiceService.RegisterSendToApprove(model, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> Info(int invoiceRegisterId)
            => CreateActionResult(await _invoiceService.Info(invoiceRegisterId));

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
        public async Task<IActionResult> Save(InvoiceRegisterSaveModel model)
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
        public async Task<IActionResult> MatchingMainService([FromQuery] InvoiceMatchingGRNModel model)
            => CreateActionResult(await _invoiceService.MatchingMainService(model));

        [HttpGet]
        public async Task<IActionResult> GetMatchingMain([FromQuery] int orderMainId)
            => CreateActionResult(await _invoiceService.GetMatchingMain(orderMainId));

        [HttpGet]
        public async Task<IActionResult> GetDetails([FromQuery] InvoiceGetDetailsModel model)
        {
            string keyCode = await _invoiceService.GetKeyCode(model.InvoiceRegisterId);
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
        public async Task<IActionResult> SaveInvoiceMatchingMain(InvoiceMathcingMain request)
        {
            int userId = Convert.ToInt32(User.Identity.Name);
            return CreateActionResult(await _invoiceService.SaveInvoiceMatchingMain(request, userId));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveInvoiceMatchingGRNs(InvoiceMatchingGRNs request)
        {
            return CreateActionResult(await _invoiceService.SaveInvoiceMatchingGRNs(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveInvoiceMatchingAdvances(InvoiceMatchingAdvance request)
        {
            return CreateActionResult(await _invoiceService.SaveInvoiceMatchingAdvances(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveInvoiceMatchingDetails(InvoiceMatchingDetail request)
        {
            return CreateActionResult(await _invoiceService.SaveInvoiceMatchingDetails(request));
        }
        
    }
}