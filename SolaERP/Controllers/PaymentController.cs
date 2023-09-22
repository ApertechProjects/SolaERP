using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Balance([FromQuery]CreateBalanceModel createBalance)
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
