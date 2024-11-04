using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VendorController : CustomBaseController
    {
        private readonly IVendorService _service;
        public VendorController(IVendorService service) => _service = service;


        [Authorize]
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> Get(int vendorId)
            => CreateActionResult(await _service.GetAsync(vendorId));



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Vendors()
            => CreateActionResult(await _service.Vendors(User.Identity.Name));

		[HttpPost]
		public async Task<IActionResult> GetByTax([FromQuery] string taxId)
		  => Ok(await _service.GetByTaxAsync(taxId));

	}
}