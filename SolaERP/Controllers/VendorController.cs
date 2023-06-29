using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
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

        //[HttpGet]
        //public async Task<IActionResult> GetWFA([FromQuery] VendorFilter filter)
        //    => CreateActionResult(await)

    }
}
