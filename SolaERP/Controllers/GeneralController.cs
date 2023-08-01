using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RazorLight.Generation;
using SolaERP.Application.Contracts.Services;
using SolaERP.Persistence.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GeneralController : CustomBaseController
    {
        private readonly IGeneralService _generalService;
        public GeneralController(IGeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet]
        public async Task<IActionResult> RejectReasonsAsync()
            => CreateActionResult(await _generalService.RejectReasons());

        [HttpGet]
        public async Task<IActionResult> GetStatusAsync()
            => CreateActionResult(await _generalService.GetStatus());


    }
}
