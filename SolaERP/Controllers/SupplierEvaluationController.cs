using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            => CreateActionResult(await _service.GetInitRegistrationAsync());
    }
}
