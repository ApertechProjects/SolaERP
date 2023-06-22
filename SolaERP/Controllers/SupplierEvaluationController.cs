using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using SolaERP.Controllers;
using System.ComponentModel.DataAnnotations;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierEvaluationController : CustomBaseController
    {
        private readonly ISupplierEvaluationService _service;
        public SupplierEvaluationController(ISupplierEvaluationService service) => _service = service;


        [HttpGet("[action]")]
        public async Task<IActionResult> InitReg()
            => CreateActionResult(await _service.GetInitRegistrationAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> NonDisclosureAgreement()
            => CreateActionResult(await _service.GetNDAAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> CodeOfBuConduct()
            => CreateActionResult(await _service.GetCOBCAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> BankDetails()
            => CreateActionResult(await _service.GetBankDetailsAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> DueDiligence()
           => CreateActionResult(await _service.GetDueDiligenceAsync(User.Identity.Name, Request.Headers.AcceptLanguage));

        [HttpGet("[action]")]
        public async Task<IActionResult> Prequalification([FromQuery] List<int> ids)
            => CreateActionResult(await _service.GetPrequalificationAsync(User.Identity.Name, ids, Request.Headers.AcceptLanguage));

        [HttpPost]
        public async Task<IActionResult> Post(SupplierRegisterCommand command)
            => CreateActionResult(await _service.AddAsync(User.Identity.Name, command));

        [HttpPost("[action]")]
        public async Task<IActionResult> Submit(SupplierRegisterCommand command)
            => CreateActionResult(await _service.SubmitAsync(User.Identity.Name, command));
    }
}
