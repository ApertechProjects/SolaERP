using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> UOM(string businessUnitCode)
            => CreateActionResult(await _uomService.GetUOMListBusinessUnitCode(businessUnitCode));
    }
}
