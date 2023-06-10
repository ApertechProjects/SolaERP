using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierEvaluationController : CustomBaseController
    {
        private readonly ISupplierEvaluationService _service;

        public SupplierEvaluationController(ISupplierEvaluationService service)
        {
            _service = service;
        }



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SupplierEvaluationGETModel model)
            => CreateActionResult(await _service.GetAllAsync(model));

        [HttpGet("[action]")]
        public async Task<IActionResult> InitReg()
            => CreateActionResult(await _service.GetInitRegistrationAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> BankDetails()
            => CreateActionResult(await _service.GetBankDetailsAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> NonDisclosureAgreement()
            => CreateActionResult(await _service.GetNDAAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> CodeOfBuConduct()
            => CreateActionResult(await _service.GetCOBCAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> DueDiligence()
           => CreateActionResult(await _service.GetDueDiligenceAsync(User.Identity.Name, Request.Headers.AcceptLanguage));

        [HttpGet("[action]")]
        public async Task<IActionResult> Prequalification()
            => CreateActionResult(await _service.GetPrequalificationAsync(User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Post(SupplierRegisterCommand command)
            => CreateActionResult(await _service.AddAsync(User.Identity.Name, command));
    }
}
