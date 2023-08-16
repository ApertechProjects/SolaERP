using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnalysisCode;
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
           => CreateActionResult(await _analysisCodeService.GetAnalysisCodesListAsync(getRequest,User.Identity.Name));

        [HttpGet("{dimensionId}")]
        public async Task<IActionResult> AnalysisCodes(int dimensionId)
         => CreateActionResult(await _analysisCodeService.GetAnalysisCodeListAsync(dimensionId, User.Identity.Name));

        [HttpGet("{dimensionId}")]
        public async Task<IActionResult> ByDimension(int dimensionId)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(dimensionId, User.Identity.Name));


        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> ByBusinessUnit(int businessUnitId)
           => CreateActionResult(await _analysisCodeService.GetByBUIdAsync(businessUnitId, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Save(List<AnalysisCodeSaveModel> analysisCodeSave)
            => CreateActionResult(await _analysisCodeService.SaveAnalysisCodeAsync(analysisCodeSave, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete(AnalysisCodeDeleteModel model)
           => CreateActionResult(await _analysisCodeService.DeleteAnalysisCodeAsync(model, User.Identity.Name));


    }
}
