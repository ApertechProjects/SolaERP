using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnalysisCodeController : CustomBaseController
    {
        private readonly IAnalysisCodeService _analysisCodeService;

        public AnalysisCodeController(IAnalysisCodeService analysisCodeService)
        {
            _analysisCodeService = analysisCodeService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAnalysisCodes(AnalysisCodeGetModel getRequest)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(getRequest));
    }
}
