using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

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

        /// <summary>
        ///Retrieve a list of all analysis codes
        /// </summary>
        /// <remarks>This endpoint retrieves a list of all analysis codes in the system.</remarks>
        [HttpPost]
        public async Task<IActionResult> GetAnalysisCodes(AnalysisCodeGetModel getRequest)
            => CreateActionResult(await _analysisCodeService.GetAnalysisCodesAsync(getRequest));

        [HttpGet]
        public async Task<IActionResult> GetAnalysisDimensionAsync()
            => CreateActionResult(await _analysisCodeService.GetAnalysisDimensionAsync());

      
    }
}
