using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Features.Queries.AnalysisCode;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AnalysisCodeController : CustomBaseController
    {
        private readonly IAnalysisCodeService _analysisCodeService;
        private readonly IMediator _mediator;

        public AnalysisCodeController(IAnalysisCodeService analysisCodeService, IMediator mediator)
        {
            _analysisCodeService = analysisCodeService;
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetAnalysisCodes([FromQuery] AnalysisCodeGetModel getRequest)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(getRequest));

        [HttpGet]
        public async Task<IActionResult> GetAnalysisDimensionAsync()
            => CreateActionResult(await _analysisCodeService.GetAnalysisDimensionAsync());


        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitAnalysisDimensions([FromQuery] GetAnalysisDimensionByBuRequest request)
            => CreateActionResult(await _mediator.Send(request));
    }
}
