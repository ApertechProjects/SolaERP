using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorController : CustomBaseController
    {
        private readonly IVendorService _service;
        public VendorController(IVendorService service) => _service = service;



        [HttpGet]
        public async Task<IActionResult> Filters()
            => CreateActionResult(await _service.GetFiltersAsync());

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> Get(int vendorId)
            => CreateActionResult(await _service.GetAsync(vendorId));

        [HttpGet("{taxId}")]
        public async Task<IActionResult> GetByTax([FromQuery] string taxId)
          => Ok(await _service.GetByTaxAsync(taxId));

        [HttpGet]
        public async Task<IActionResult> Vendors()
           => CreateActionResult(await _service.Vendors(User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> GetAll(VendorAllCommandRequest request)
            => CreateActionResult(await _service.GetAllAsync(User.Identity.Name, request));

        [HttpPost]
        public async Task<IActionResult> GetWFA(VendorFilter filter)
            => CreateActionResult(await _service.GetWFAAsync(User.Identity.Name, filter));

        [HttpPost]
        public async Task<IActionResult> GetDraft(VendorFilter filter)
            => CreateActionResult(await _service.GetDraftAsync(User.Identity.Name, filter));

        [HttpPost]
        public async Task<IActionResult> GetHeld(VendorFilter filter)
         => CreateActionResult(await _service.GetHeldAsync(User.Identity.Name, filter));

        [HttpPost]
        public async Task<IActionResult> GetApproved(VendorFilter filter)
         => CreateActionResult(await _service.GetApprovedAsync(User.Identity.Name, filter.Text));

        [HttpPost]
        public async Task<IActionResult> GetRejected(VendorFilter filter)
         => CreateActionResult(await _service.GetRejectedAsync(User.Identity.Name, filter));

        [HttpPost]
        public async Task<IActionResult> SendToApprove(VendorSendToApproveRequest request)
            => CreateActionResult(await _service.SendToApproveAsync(request));

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(VendorStatusModel model)
                   => CreateActionResult(await _service.ChangeStatusAsync(model, User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Save([FromForm] VendorCardDto vendor)
            => CreateActionResult(await _service.SaveAsync(User.Identity.Name, vendor));

        [HttpPost]
        public async Task<IActionResult> Approve(VendorApproveModel model)
            => CreateActionResult(await _service.ApproveAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> Delete(VendorDeleteModel model)
            => CreateActionResult(await _service.DeleteAsync(User.Identity.Name, model));

    }
}
