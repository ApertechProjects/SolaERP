using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RFQController : CustomBaseController
    {
        private readonly IRfqService _service;
        public RFQController(IRfqService service) => _service = service;


        [HttpGet("[action]/{rfqMainId}")]
        public async Task<IActionResult> Get(int rfqMainId)
            => CreateActionResult(await _service.GetRFQAsync(User.Identity.Name, rfqMainId));

		[HttpGet("[action]")]
		public async Task<IActionResult> GetConversionList([FromQuery] int businessUnitId, [FromQuery] string itemCode)
			=> CreateActionResult(await _service.GetPUOMAsync(businessUnitId, itemCode));

	}
}
