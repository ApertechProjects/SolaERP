using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RFQController : CustomBaseController
    {
        private readonly IRfqService _service;
        private readonly IUnitOfWork _unitOfWork;

        public RFQController(IRfqService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetVendorsToSend([FromQuery] int businessCategoryId)
            => CreateActionResult(await _service.GetRFQVendorsAsync(businessCategoryId));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBusinessCategories()
            => CreateActionResult(await _service.GetBuCategoriesAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] RfqAllFilter filter)
            => CreateActionResult(await _service.GetAllAsync(filter));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetInProgress([FromQuery] RFQFilterBase filter)
            => CreateActionResult(await _service.GetInProgressAsync(filter, User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetDrafts([FromQuery] RfqFilter filter)
            => CreateActionResult(await _service.GetDraftsAsync(filter, User.Identity.Name));

        [HttpGet("[action]/{rfqMainId}")]
        public async Task<IActionResult> Get(int rfqMainId)
            => CreateActionResult(await _service.GetRFQAsync(User.Identity.Name, rfqMainId));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetRequestForRFQ([FromQuery] RFQRequestModel model)
            => CreateActionResult(await _service.GetRequestsForRFQ(User.Identity.Name, model));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSingleSourceReasons()
            => CreateActionResult(await _service.GetSingleSourceReasonsAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> GetConversionList([FromQuery] int businessUnitId, [FromQuery] string itemCode)
            => CreateActionResult(await _service.GetPUOMAsync(businessUnitId, itemCode));

        [HttpPost]
        public async Task<IActionResult> Save(RfqSaveCommandRequest request)
            => CreateActionResult(await _service.SaveRfqAsync(request, User.Identity.Name));

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveRFQVendors(RFQVendorIUDDto dto)
            => CreateActionResult(await _service.RFQVendorIUDAsync(dto, User.Identity.Name));

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeRFQStatus(RfqChangeStatusModel model)
            => CreateActionResult(await _service.ChangeRFQStatusAsync(model, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] List<int> rfqMainId)
            => CreateActionResult(await _service.DeleteAsync(rfqMainId, User.Identity.Name));

        [HttpGet("[action]/{rfqMainId}")]
        public async Task<IActionResult> GetRfqVendors(int rfqMainId)
            => CreateActionResult(await _service.GetRfqVendors(rfqMainId));

        [HttpGet("[action]")]
        public async Task UpdateRFQStatusToClose()
        {
            await _service.GetRFQDeadlineFinished();
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> ExtendRfqDeadline(RfqExtendDeadlineRequest request) =>
            CreateActionResult(await _service.ExtendRfqDeadlineAsync(request, Convert.ToInt32(User.Identity.Name)));

    }
}