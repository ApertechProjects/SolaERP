using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GNRLConfigController : CustomBaseController
    {
        private readonly IGNRLConfigService _gnrlConfigService;

        public GNRLConfigController(IGNRLConfigService gnrlConfigService)
        {
            _gnrlConfigService = gnrlConfigService;
        }

        [HttpGet("{businessUnitId:int}")]
        public async Task<IActionResult> GetList([FromRoute] int businessUnitId)
            => CreateActionResult(await _gnrlConfigService.GetGNRLListByBusinessUnitId(businessUnitId));
    }
}