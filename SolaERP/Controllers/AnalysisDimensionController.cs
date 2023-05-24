using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Features.Queries.AnalysisCode;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnalysisDimensionController : CustomBaseController
    {
        //private readonly IAnalysisDimensionService _analysisDimensionService;
        public AnalysisDimensionController(
            //IAnalysisDimensionService analysisDimensionService
            )
        {


        }

        //[HttpGet]
        //public async Task<IActionResult> GetAnalysisDimensionAsync()
        //   => CreateActionResult(await _analysisDimensionService.GetAnalysisDimensionAsync());


        //[HttpGet]
        //public async Task<IActionResult> GetAnalysisDimensionsByBusinessUnitIdAsync([FromQuery] GetAnalysisDimensionByBuRequest request)
        //    => CreateActionResult(await _mediator.Send(request));

    }
}
