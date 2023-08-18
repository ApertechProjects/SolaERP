using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Controllers;
using SolaERP.Persistence.Services;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidComparisonController : CustomBaseController
    {
        private readonly IBidComparisonService _bidComparisonService;
        public BidComparisonController(IBidComparisonService bidComparisonService) => _bidComparisonService = bidComparisonService;

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] BidComparisonAllFilterDto filter)
           => CreateActionResult(await _bidComparisonService.GetBidComparisonAllAsync(filter));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComparison([FromQuery] BidComparisonFilterDto filter)
           => CreateActionResult(await _bidComparisonService.GetBidComparisonAsync(filter));

        [HttpPost("[action]")]
        public async Task<IActionResult> AddComparison(BidComparisonCreateDto comparison)
           => CreateActionResult(await _bidComparisonService.SaveBidComparisonAsync(comparison));

        [HttpPost("[action]")]
        public async Task<IActionResult> ApproveComparison(BidComparisonApproveDto approve)
           => CreateActionResult(await _bidComparisonService.ApproveBidComparisonAsync(approve));
        
        [HttpPost("[action]")]
        public async Task<IActionResult> SendComparisonToApprove(BidComparisonSendToApproveDto bidComparisonSendToApproveDto)
           => CreateActionResult(await _bidComparisonService.SendComparisonToApproveAsync(bidComparisonSendToApproveDto));

    }
}
