using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class PaymentController : CustomBaseController
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> WaitingForApproval([FromQuery] PaymentGetModel payment)
            => CreateActionResult(await _paymentService.WaitingForApproval(User.Identity.Name, payment));

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] PaymentGetModel payment)
          => CreateActionResult(await _paymentService.All(User.Identity.Name, payment));

        [HttpGet]
        public async Task<IActionResult> Draft([FromQuery] PaymentGetModel payment)
          => CreateActionResult(await _paymentService.Draft(User.Identity.Name, payment));

        [HttpGet]
        public async Task<IActionResult> Approved([FromQuery] PaymentGetModel payment)
          => CreateActionResult(await _paymentService.Approved(User.Identity.Name, payment));

        [HttpGet]
        public async Task<IActionResult> Held([FromQuery] PaymentGetModel payment)
          => CreateActionResult(await _paymentService.Held(User.Identity.Name, payment));

        [HttpGet]
        public async Task<IActionResult> Rejected([FromQuery] PaymentGetModel payment)
          => CreateActionResult(await _paymentService.Rejected(User.Identity.Name, payment));

        [HttpGet]
        public async Task<IActionResult> Bank([FromQuery] PaymentGetModel payment)
            => CreateActionResult(await _paymentService.Bank(User.Identity.Name, payment));

        [HttpPost]
        public async Task<IActionResult> SendToApprove(SendToApproveModel sendToApprove)
            => CreateActionResult(await _paymentService.SendToApprove(User.Identity.Name, sendToApprove.Ids));

        [HttpGet]
        public async Task<IActionResult> Balance([FromQuery] CreateBalanceModel createBalance)
            => CreateActionResult(await _paymentService.CreateBalanceAsync(createBalance));

        [HttpGet]
        public async Task<IActionResult> Advance([FromQuery] CreateAdvanceModel createAdvance)
            => CreateActionResult(await _paymentService.CreateAdvanceAsync(createAdvance));

        [HttpGet]
        public async Task<IActionResult> Order([FromQuery] CreateOrderModel createOrder)
            => CreateActionResult(await _paymentService.CreateOrderAsync(createOrder));


        [HttpGet("{paymentDocumentMainId}")]
        public async Task<IActionResult> Info(int paymentDocumentMainId)
            => CreateActionResult(await _paymentService.Info(paymentDocumentMainId));

        [HttpGet]
        public IActionResult DocumentTypes()
           => CreateActionResult(_paymentService.DocumentTypes());

        [HttpGet("{businessUnitId}/{vendorCode}")]
        public async Task<IActionResult> VendorBalance(int businessUnitId, string vendorCode)
            => CreateActionResult(await _paymentService.VendorBalance(businessUnitId, vendorCode));

        [HttpPost]
        public async Task<IActionResult> Save(PaymentDocumentSaveModel model)
            => CreateActionResult(await _paymentService.Save(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> Delete(PaymentDocumentDeleteModel model)
            => CreateActionResult(await _paymentService.Delete(model, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(PaymentChangeStatusModel model)
            => CreateActionResult(await _paymentService.ChangeStatus(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> Retrieve(PaymentOperationModel model)
            => CreateActionResult(await _paymentService.PaymentOperation(User.Identity.Name, model, PaymentOperations.Retrieve));

        [HttpPost]
        public async Task<IActionResult> ReturnToApproved(PaymentOperationModel model)
           => CreateActionResult(await _paymentService.PaymentOperation(User.Identity.Name, model, PaymentOperations.SendToApproved));

        [HttpPost]
        public async Task<IActionResult> SendToBank(PaymentOperationModel model)
           => CreateActionResult(await _paymentService.PaymentOperation(User.Identity.Name, model, PaymentOperations.SendToBank));

        [HttpGet]
        public async Task<IActionResult> PaymentRequest([FromQuery] PaymentRequestGetModel model)
            => CreateActionResult(await _paymentService.PaymentRequest(model));

        [HttpPost]
        public async Task<IActionResult> PaymentOrderLoad(PaymentOrderParamModel paymentOrder)
            => CreateActionResult(await _paymentService.PaymentOrderLoad(paymentOrder));

        [HttpPost]
        public async Task<IActionResult> PaymentOrderTransaction(PaymentOrderTransactionModel model)
            => CreateActionResult(await _paymentService.PaymentOrderTransaction(model));

        [HttpGet]
        public async Task<IActionResult> BankAccountList(int businessUnitId)
            => CreateActionResult(await _paymentService.BankAccountList(businessUnitId));

      
        [HttpPost]
        public async Task<IActionResult> PaymentOrderPostData(PaymentOrderPostModel model)
            => CreateActionResult(await _paymentService.PaymentOrderPostData(model, User.Identity.Name));
    }
}
