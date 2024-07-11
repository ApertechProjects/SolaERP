using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BidController : CustomBaseController
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService) => _bidService = bidService;

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] BidAllFilterDto filter)
            => CreateActionResult(await _bidService.GetAllAsync(filter));

        [HttpGet("[action]/Details")]
        public async Task<IActionResult> Get([FromQuery] BidDetailsFilterDto filter)
            => CreateActionResult(await _bidService.GetBidDetailsAsync(filter));

        [HttpGet("[action]/{bidMainId}")]
        public async Task<IActionResult> GetCard(int bidMainId)
            => CreateActionResult(await _bidService.GetMainLoadAsync(bidMainId));

        [HttpGet("[action]/{businessUnitId}")]
        public async Task<IActionResult> GetRFQList(int businessUnitId)
            => CreateActionResult(await _bidService.GetRfqListAsync(User.Identity.Name, businessUnitId));

        //[HttpGet("[action]/{bidMainId}")]
        //public async Task<IActionResult> GetCardWithLists(int bidMainId)
        //    => CreateActionResult(await _bidService.GetBidCardAsync(bidMainId));

        [HttpPost]
        public async Task<IActionResult> Save(BidMainDto bidMain)
            => CreateActionResult(await _bidService.SaveBidMainAsync(bidMain, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> Delete(int bidMainId)
            => CreateActionResult(await _bidService.DeleteBidMainAsync(bidMainId, User.Identity.Name));

        [HttpPost("[action]")]
        public async Task<IActionResult> Disqualify(BidDisqualifyDto dto)
            => CreateActionResult(await _bidService.BidDisqualifyAsync(dto, User.Identity.Name));

      
    }
}