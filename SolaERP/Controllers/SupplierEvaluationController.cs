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
        public async Task<IActionResult> CodeOfBuConduct(int? vendorId = null)
            => CreateActionResult(await _service.GetCOBCAsync(User.Identity.Name, vendorId));

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

        [HttpGet("[action]")]
        public async Task<IActionResult> Prequalification(int id, int? vendorId = null)
          => CreateActionResult(await _service.GetPrequalificationAsync2(User.Identity.Name, id,
              Request.Headers.AcceptLanguage, vendorId));

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Post([FromBody] SupplierRegisterCommand command,
            [FromQuery] bool isRevise = false)
        {
            return CreateActionResult(await _service.AddAsync(User.Identity.Name, "", command, isRevise));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Submit([FromBody] SupplierRegisterCommand command,
            [FromQuery] bool isRevise = false)
        {
            return CreateActionResult(await _service.SubmitAsync(User.Identity.Name, "", command, isRevise));
        }

        [HttpPost("[action]")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Post2([FromBody] SupplierRegisterCommand2 command,
         [FromQuery] bool isRevise = false)
        {
            return CreateActionResult(await _service.AddAsync2(User.Identity.Name, command, isRevise));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Submit2([FromBody] SupplierRegisterCommand2 command,
            [FromQuery] bool isRevise = false)
        {
            return CreateActionResult(await _service.SubmitAsync2(User.Identity.Name, command, isRevise));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateVendor(string taxId)
            => CreateActionResult(await _service.UpdateVendor(User.Identity.Name, taxId));
    }
}