using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorController : CustomBaseController
    {
        private readonly IVendorService _service;

        public VendorController(IVendorService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> Filters()
            => CreateActionResult(await _service.GetFiltersAsync());

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] VendorFilter filter, [FromQuery] Status status, [FromQuery] ApprovalStatus approval)
            => CreateActionResult(await _service.GetAllAsync(User.Identity.Name, filter, status, approval));

        [HttpGet]
        public async Task<IActionResult> GetWFA([FromQuery] VendorFilter filter)
            => CreateActionResult(await _service.GetWFAAsync(User.Identity.Name, filter));

    }
}
