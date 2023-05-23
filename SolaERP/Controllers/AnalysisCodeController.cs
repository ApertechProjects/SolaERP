using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Features.Queries.AnalysisCode;
using SolaERP.Application.Models;

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
        public async Task<IActionResult> AnalysisCodes([FromQuery] AnalysisCodeGetModel getRequest)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(getRequest));

        [HttpGet("{analysisCodeId}")]
        public async Task<IActionResult> AnalysisCodes(int analysisCodeId)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(analysisCodeId, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Save(AnalysisDto analysisDto)
            => CreateActionResult(await _analysisCodeService.SaveAnalysisCodeAsync(analysisDto, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete(int analysisCodeId)
           => CreateActionResult(await _analysisCodeService.DeleteAnalysisCodeAsync(analysisCodeId));

        [HttpGet]
        public async Task<IActionResult> GetAnalysisDimensionAsync()
            => CreateActionResult(await _analysisCodeService.GetAnalysisDimensionAsync());


        [HttpGet]
        public async Task<IActionResult> GetAnalysisDimensionsByBusinessUnitIdAsync([FromQuery] GetAnalysisDimensionByBuRequest request)
            => CreateActionResult(await _mediator.Send(request));

        [HttpGet("{dimensionId}")]
        public async Task<IActionResult> GetAnalysisCodesByDimensionIdAsync(int dimensionId)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesByDimensionIdAsync(dimensionId));
    }
}
