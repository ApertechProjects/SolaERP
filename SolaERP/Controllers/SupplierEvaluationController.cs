using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using SolaERP.Controllers;
using Language = SolaERP.Application.Enums.Language;

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
        public async Task<IActionResult> DueDiligence(Language language)
            => CreateActionResult(await _service.GetDueDiligenceAsync(language));

        [HttpGet("[action]")]
        public async Task<IActionResult> Prequalification()
            => CreateActionResult(await _service.GetPrequalificationAsync(User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Post(SupplierRegisterCommand command)
            => CreateActionResult(await _service.AddAsync(User.Identity.Name, command));
    }
}
