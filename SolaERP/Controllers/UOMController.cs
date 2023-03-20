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

        /// <summary>
        ///This endpoint returns a list of all available unit of measure codes.
        /// </summary>
        /// <remarks>The GetAllUOMList endpoint returns a list of all the available unit of measure codes in the system. This endpoint can be used to retrieve the list of available UOM codes and their descriptions for use in other parts of the system.</remarks>
        ///<param name="businessUnitCode">The unique identifier of the business unit for which to retrieve currency codes.</param>
        [HttpGet("{businessUnitCode}")]
        public async Task<IActionResult> GetUOMListBusinessUnitCode(string businessUnitCode)
            => CreateActionResult(await _uomService.GetUOMListBusinessUnitCode(businessUnitCode));
    }
}
