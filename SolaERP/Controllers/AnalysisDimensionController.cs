using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Models;
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
        public async Task<IActionResult> ByAnalysisDimension(int analysisDimensionId)
           => CreateActionResult(await _analysisDimensionService.ByDimensionIdAsync(analysisDimensionId, User.Identity.Name));


        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> ByBusinessUnit(int businessUnitId)
            => CreateActionResult(await _analysisDimensionService.ByBUIdAsync(businessUnitId, User.Identity.Name));


        [HttpPost]
        public async Task<IActionResult> Save(List<AnalysisDimensionDto> analysisDimension)
            => CreateActionResult(await _analysisDimensionService.SaveAsync(analysisDimension, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete(AnalysisDimensionDeleteModel model)
            => CreateActionResult(await _analysisDimensionService.DeleteAsync(model, User.Identity.Name));

    }
}
