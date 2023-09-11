using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BidComparisonController : CustomBaseController
    {
        private readonly IBidComparisonService _bidComparisonService;
        public BidComparisonController(IBidComparisonService bidComparisonService) => _bidComparisonService = bidComparisonService;

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] BidComparisonAllFilterDto filter)
        {
           return CreateActionResult(await _bidComparisonService.GetBidComparisonAllAsync(filter));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparison(int rfqMainId)
        {
            BidComparisonFilterDto filter = new()
            {
                UserId = Convert.ToInt32(User.Identity.Name),
                RFQMainId = rfqMainId
            };
            return CreateActionResult(await _bidComparisonService.GetBidComparisonAsync(filter));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddComparison(BidComparisonCreateDto comparison)
        {
            comparison.UserId = Convert.ToInt32(User.Identity.Name);
            return CreateActionResult(await _bidComparisonService.SaveBidComparisonAsync(comparison));

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ApproveComparison(BidComparisonApproveDto approve)
        {
            approve.UserId = Convert.ToInt32(User.Identity.Name);
            approve.ApproveStatus = 1;
            return CreateActionResult(await _bidComparisonService.ApproveBidComparisonAsync(approve));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RejectComparison(BidComparisonApproveDto approve)
        {
            approve.UserId = Convert.ToInt32(User.Identity.Name);
            approve.ApproveStatus = 2;
            return CreateActionResult(await _bidComparisonService.ApproveBidComparisonAsync(approve));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendComparisonToApprove(BidComparisonSendToApproveDto bidComparisonSendToApproveDto)
        {
            bidComparisonSendToApproveDto.UserId = Convert.ToInt32(User.Identity.Name);
            return CreateActionResult(await _bidComparisonService.SendComparisonToApproveAsync(bidComparisonSendToApproveDto));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparisonWFA([FromQuery] BidComparisonWFAFilterDto filterDto)
        {
            return CreateActionResult(await _bidComparisonService.GetComparisonWFA(filterDto));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparisonDraft([FromQuery] BidComparisonDraftFilterDto filterDto)
        {
            return CreateActionResult(await _bidComparisonService.GetComparisonDraft(filterDto));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparisonHeld([FromQuery] BidComparisonHeldFilterDto filterDto)
        {
            return CreateActionResult(await _bidComparisonService.GetComparisonHeld(filterDto));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparisonMyCharts([FromQuery] BidComparisonMyChartsFilterDto filterDto)
        {
            return CreateActionResult(await _bidComparisonService.GetComparisonMyCharts(filterDto, User.Identity.Name));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparisonNotReleased([FromQuery] BidComparisonNotReleasedFilterDto filterDto)
        {
            return CreateActionResult(await _bidComparisonService.GetComparisonNotReleased(filterDto));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparisonRejected([FromQuery] BidComparisonRejectedFilterDto filterDto)
        {
            return CreateActionResult(await _bidComparisonService.GetComparisonRejected(filterDto));
        }
    }
}
