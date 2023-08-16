using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RFQController : CustomBaseController
    {
        private readonly IRfqService _service;
        public RFQController(IRfqService service) => _service = service;


        [HttpGet("{businessCategoryId}")]
        public async Task<IActionResult> GetRFQVendors(int businessCategoryId)
            => CreateActionResult(await _service.GetRFQVendorsAsync(businessCategoryId));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBusinessCategories()
            => CreateActionResult(await _service.GetBuCategoriesAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] RfqAllFilter filter)
            => CreateActionResult(await _service.GetAllAsync(filter));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetInProgress([FromQuery] RFQFilterBase filter)
            => CreateActionResult(await _service.GetInProgressAsync(filter));

        [HttpGet]
        public async Task<IActionResult> GetDrafts([FromQuery] RfqFilter filter)
            => CreateActionResult(await _service.GetDraftsAsync(filter));

        [HttpGet("[action]/{rfqMainId}")]
        public async Task<IActionResult> Get(int rfqMainId)
            => CreateActionResult(await _service.GetRFQAsync(User.Identity.Name, rfqMainId));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetRequestForRFQ([FromQuery] RFQRequestModel model)
            => CreateActionResult(await _service.GetRequestsForRFQ(User.Identity.Name, model));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSingleSourceReasons()
            => CreateActionResult(await _service.GetSingleSourceReasonsAsync());

        [HttpPost]
        public async Task<IActionResult> Save(RfqSaveCommandRequest request)
            => CreateActionResult(await _service.SaveRfqAsync(request, User.Identity.Name));

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeRFQStatus(RfqChangeStatusModel model)
            => CreateActionResult(await _service.ChangeRFQStatusAsync(model, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] List<int> rfqMainId)
            => CreateActionResult(await _service.DeleteAsync(rfqMainId, User.Identity.Name));

    }
}
