using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Support;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupportController : CustomBaseController
    {
        private readonly ISupportService _service;
        public SupportController(ISupportService service) => _service = service;

        [HttpPost("[action]")]
        public async Task<IActionResult> Save(SupportSaveDto dto)
            => CreateActionResult(await _service.Save(dto, User.Identity.Name));
	}
}
