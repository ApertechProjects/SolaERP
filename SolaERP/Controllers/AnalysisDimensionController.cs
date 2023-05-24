using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnalysisDimensionController : CustomBaseController
    {
        private readonly IAnalysisDimensionService _analysisDimensionService;
        public AnalysisDimensionController(IAnalysisDimensionService analysisDimensionService)
        {
            _analysisDimensionService = analysisDimensionService;

        }

        [HttpGet("{analysisDimensionId}")]
        public async Task<IActionResult> ByAnalysisDimensionId(int analysisDimensionId)
           => CreateActionResult(await _analysisDimensionService.ByAnalysisDimensionId(analysisDimensionId, User.Identity.Name));


        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> ByBusinessUnitId(int businessUnitId)
            => CreateActionResult(await _analysisDimensionService.ByBusinessUnitId(businessUnitId, User.Identity.Name));


        [HttpPost]
        public async Task<IActionResult> Save(AnalysisDimensionDto analysisDimension)
            => CreateActionResult(await _analysisDimensionService.Save(analysisDimension, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete(int businessUnitId)
            => CreateActionResult(await _analysisDimensionService.Delete(businessUnitId, User.Identity.Name));

    }
}
