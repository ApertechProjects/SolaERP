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
        private readonly IBusinessUnitService _businessUnitService;
        public UOMController(IUOMService uomService, IBusinessUnitService businessUnitService)
        {
            _uomService = uomService;
            _businessUnitService = businessUnitService;
        }


        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> UOM(int businessUnitId)
        {
            var businessUnitCode = await _businessUnitService.GetBusinessUnitCode(businessUnitId);
            return CreateActionResult(await _uomService.GetUOMListBusinessUnitCode(businessUnitId));
        }
    }
}
