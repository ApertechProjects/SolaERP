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

    }
}
