using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnalysisStructure;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;
using SolaERP.Controllers;
using SolaERP.Persistence.Services;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalysisStructureController : CustomBaseController
    {
        private readonly IAnalysisStructureService _analysisService;

        public AnalysisStructureController(IAnalysisStructureService analysisStructureService)
        {
            _analysisService = analysisStructureService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int businessUnitId, int procedureId)
            => CreateActionResult(await _analysisService.GetByBUAsync(businessUnitId, procedureId, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Add(List<AnalysisStructureDto> model)
           => CreateActionResult(await _analysisService.SaveAsync(model, User.Identity.Name));


        [HttpDelete]
        public async Task<IActionResult> Delete(AnalysisStructureDeleteModel model)
         => CreateActionResult(await _analysisService.DeleteAsync(model, User.Identity.Name));
    }

}
