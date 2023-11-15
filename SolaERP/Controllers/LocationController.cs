using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LocationController : CustomBaseController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("{businessUnitId:int}")]
        public async Task<IActionResult> GetLocationListAsync(int businessUnitId)
            => CreateActionResult(await _locationService.GetAllByBusinessUnitId(businessUnitId));
    }
}