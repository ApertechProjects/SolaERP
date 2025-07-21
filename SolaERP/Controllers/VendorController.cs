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

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Save2(VendorCardDto2 vendor)
			=> CreateActionResult(await _service.SaveAsync2(User.Identity.Name, vendor));
		
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> RFQVendorResponseChangeStatus([FromBody] VendorRFQStatusChangeRequest request)
			=> CreateActionResult(await _service.RFQVendorResponseChangeStatus(
				request.RFQMainId, request.Status, request.VendorCode, request.RejectReasonId, request.Comment
			));

		[Authorize]
		[HttpGet("{vendorCode}")]
		public async Task<IActionResult> GetVendorRFQList([FromRoute] string vendorCode)
			=> CreateActionResult(await _service.GetVendorRFQList(vendorCode, User.Identity.Name));
	}
}