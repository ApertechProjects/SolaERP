using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UOMController : CustomBaseController
    {
        private readonly IUOMService _uomService;
        public UOMController(IUOMService uomService)
        {
            _uomService = uomService;
        }

        [HttpGet("{businessUnitCode}")]
        public async Task<IActionResult> GetUOMListBusinessUnitCode(string businessUnitCode)
            => CreateActionResult(await _uomService.GetUOMListBusinessUnitCode(businessUnitCode));
    }
}
