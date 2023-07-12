using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Controllers;
using SolaERP.Persistence.Services;

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

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> Get(int vendorId)
            => CreateActionResult(await _service.GetVendorCard(vendorId));

        [HttpGet("{taxId}")]
        public async Task<IActionResult> GetByTax(string taxId)
          => Ok(await _service.GetByTaxAsync(taxId));

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
        public async Task<IActionResult> SendToApprove(int vendorId)
            => CreateActionResult(await _service.SendToApproveAsync(vendorId));

        [HttpPost]
        public async Task<IActionResult> Approve(VendorApproveModel model)
            => CreateActionResult(await _service.ApproveAsync(User.Identity.Name, model));



    }
}
