using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ITokenHandler _tokenHandler;

        public SupplierEvaluationController(ISupplierEvaluationService service, ITokenHandler tokenHandler)
        {
            _service = service;
            _tokenHandler = tokenHandler;
        }

        [HttpGet]
        public async Task<IActionResult> InitReg(int? vendorId = null)
            => CreateActionResult(await _service.GetInitRegistrationAsync(User.Identity.Name, vendorId));

        [HttpGet("[action]")]
        public async Task<IActionResult> NonDisclosureAgreement(int? vendorId = null)
            => CreateActionResult(await _service.GetNDAAsync(User.Identity.Name, vendorId));

        [HttpGet("[action]")]
        public async Task<IActionResult> CodeOfBuConduct()
            => CreateActionResult(await _service.GetCOBCAsync(User.Identity.Name));

        [HttpGet("[action]")]
        public async Task<IActionResult> BankDetails(int? vendorId = null)
            => CreateActionResult(await _service.GetBankDetailsAsync(User.Identity.Name, vendorId));

        [HttpGet("[action]")]
        public async Task<IActionResult> DueDiligence(int? vendorId = null)
            => CreateActionResult(await _service.GetDueDiligenceAsync(User.Identity.Name,
                Request.Headers.AcceptLanguage, vendorId));

        [HttpPost("[action]")]
        public async Task<IActionResult> Prequalification([FromQuery] List<int> ids,
            [FromQuery] int? vendorId = null)
            => CreateActionResult(await _service.GetPrequalificationAsync(User.Identity.Name, ids,
                Request.Headers.AcceptLanguage, vendorId));

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Post([FromBody] SupplierRegisterCommand command,
            [FromQuery] bool isRevise = false)
        {
            var token = _tokenHandler.GetAccessToken();
            return CreateActionResult(await _service.AddAsync(User.Identity.Name, token, command, isRevise));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Submit([FromBody] SupplierRegisterCommand command,
            [FromQuery] bool isRevise = false)
        {
            var token = _tokenHandler.GetAccessToken();
            return CreateActionResult(await _service.SubmitAsync(User.Identity.Name, token, command, isRevise));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateVendor(string taxId)
            => CreateActionResult(await _service.UpdateVendor(User.Identity.Name, taxId));
    }
}