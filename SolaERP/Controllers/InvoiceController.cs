using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
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
        public async Task<IActionResult> RegisterLoadGRN(int invoiceRegisterId)
            => CreateActionResult(await _invoiceService.RegisterLoadGRN(invoiceRegisterId));

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
    }
}