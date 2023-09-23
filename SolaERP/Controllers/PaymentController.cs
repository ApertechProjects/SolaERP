using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        public async Task<IActionResult> SendToApprove(int paymentDocumentMainId)
          => CreateActionResult(await _paymentService.SendToApprove(User.Identity.Name, paymentDocumentMainId));

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
    }
}
