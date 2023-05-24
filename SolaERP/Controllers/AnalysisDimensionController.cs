using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
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

        [HttpGet]
        public async Task<IActionResult> ByAnalysisDimensionId(int analysisDimensionId)
           => CreateActionResult(await _analysisDimensionService.ByAnalysisDimensionId(analysisDimensionId, User.Identity.Name));


        //[HttpGet]
        //public async Task<IActionResult> GetAnalysisDimensionsByBusinessUnitIdAsync([FromQuery] GetAnalysisDimensionByBuRequest request)
        //    => CreateActionResult(await _mediator.Send(request));

    }
}
