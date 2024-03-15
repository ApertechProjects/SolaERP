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
        [HttpGet]
        public async Task<IActionResult> Filters()
            => CreateActionResult(await _service.GetFiltersAsync());

        [Authorize]
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> Get(int vendorId)
            => CreateActionResult(await _service.GetAsync(vendorId));


        [HttpPost]
        public async Task<IActionResult> GetByTax([FromQuery] string taxId)
            => Ok(await _service.GetByTaxAsync(taxId));

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Vendors()
            => CreateActionResult(await _service.Vendors(User.Identity.Name));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetAll(VendorAllCommandRequest request)
            => CreateActionResult(await _service.GetAllAsync(User.Identity.Name, request));

        [HttpPost]
        public async Task<IActionResult> GetWFA(VendorFilter filter)
            => CreateActionResult(await _service.GetWFAAsync(User.Identity.Name, filter));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetDraft(VendorFilter filter)
            => CreateActionResult(await _service.GetDraftAsync(User.Identity.Name, filter));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetHeld(VendorFilter filter)
            => CreateActionResult(await _service.GetHeldAsync(User.Identity.Name, filter));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetApproved(VendorFilter filter)
            => CreateActionResult(await _service.GetApprovedAsync(User.Identity.Name, filter.Text));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetRejected(VendorFilter filter)
            => CreateActionResult(await _service.GetRejectedAsync(User.Identity.Name, filter));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendToApprove(VendorSendToApproveRequest request)
            => CreateActionResult(await _service.SendToApproveAsync(request));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(VendorStatusModel model)
            => CreateActionResult(await _service.ChangeStatusAsync(model, User.Identity.Name));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save([FromForm] VendorCardDto vendor)
            => CreateActionResult(await _service.SaveAsync(User.Identity.Name, vendor));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save2(VendorCardDto2 vendor)
           => CreateActionResult(await _service.SaveAsync2(User.Identity.Name, vendor));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Approve(VendorApproveModel model)
            => CreateActionResult(await _service.ApproveAsync(User.Identity.Name, model));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(VendorDeleteModel model)
            => CreateActionResult(await _service.DeleteAsync(User.Identity.Name, model));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> HasVendorName([FromBody] VendorNameDto vendorNameDto)
            => CreateActionResult(await _service.HasVendorName(vendorNameDto.VendorName, User.Identity.Name));

        [Authorize]
        [HttpGet("{vendorCode}")]
        public async Task<IActionResult> GetVendorRFQList([FromRoute] string vendorCode)
            => CreateActionResult(await _service.GetVendorRFQList(vendorCode, User.Identity.Name));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RFQVendorResponseChangeStatus([FromBody] VendorRFQStatusChangeRequest request)
            => CreateActionResult(await _service.RFQVendorResponseChangeStatus(
                request.RFQMainId, request.Status, request.VendorCode
            ));
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TransferToIntegration(CreateVendorRequest request)
        {
            request.UserId = Convert.ToInt32(User.Identity.Name);
            return CreateActionResult(await _service.TransferToIntegration(request));
        }
    }
}