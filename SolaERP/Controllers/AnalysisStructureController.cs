using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalysisStructureController : ControllerBase
    {
        private readonly IAnalysisStructureService _analysisService;

        public AnalysisStructureController(IAnalysisStructureService analysisStructureService)
        {
            _analysisService = analysisStructureService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnalysisStructure>> GetByIdAsync(int id)
        {
            var analysisStructure = await _analysisService.GetByIdAsync(id);
            if (analysisStructure == null)
            {
                return NotFound();
            }
            return analysisStructure;
        }

        [HttpGet("{buId}/{procedureId}")]
        public async Task<ActionResult<AnalysisStructureWithBu>> GetByBUAsync(int buId, int procedureId)
        {
            var analysisStructure = await _analysisService.GetByBUAsync(buId, procedureId, User.Identity.Name);
            if (analysisStructure == null)
            {
                return NotFound();
            }
            return analysisStructure;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAsync(AnalysisStructureSaveModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.UserId = Convert.ToInt32(User.Identity.Name);
            var result = await _analysisService.AddAsync(model);
            if (!result)
            {
                return StatusCode(500); // or any appropriate status code for failure
            }

            return Ok(true);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateAsync(AnalysisStructureDeleteModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.UserId = Convert.ToInt32(User.Identity.Name);
            var result = await _analysisService.UpdateAsync(model);
            if (!result)
            {
                return StatusCode(500); // or any appropriate status code for failure
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> RemoveAsync(int id)
        {
            var result = await _analysisService.RemoveAsync(id, Convert.ToInt32(User.Identity.Name));
            if (!result)
            {
                return StatusCode(500); // or any appropriate status code for failure
            }

            return Ok(true);
        }
    }

}
