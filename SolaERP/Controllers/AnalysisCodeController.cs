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

        public AnalysisCodeController(IAnalysisCodeService analysisCodeService)
        {
            _analysisCodeService = analysisCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> AnalysisCodes([FromQuery] AnalysisCodeGetModel getRequest)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(getRequest));

        [HttpGet("{analysisCodeId}")]
        public async Task<IActionResult> AnalysisCodes(int analysisCodeId)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(analysisCodeId, User.Identity.Name));

        [HttpGet("{dimensionId}")]
        public async Task<IActionResult> AnalysisCodesByDimension(int dimensionId)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesByDimensionIdAsync(dimensionId));

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> AnalysisCodesByBusinessUnit(int businessUnitId)
           => CreateActionResult(await _analysisCodeService.GetAnalysisCodesByBusinessUnitIdAsync(businessUnitId, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Save(AnalysisCodeSaveModel analysisCodeSave)
            => CreateActionResult(await _analysisCodeService.SaveAnalysisCodeAsync(analysisCodeSave, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete(int analysisCodeId)
           => CreateActionResult(await _analysisCodeService.DeleteAnalysisCodeAsync(analysisCodeId, User.Identity.Name));

    }
}
