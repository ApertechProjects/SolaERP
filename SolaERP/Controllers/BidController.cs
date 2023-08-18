using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    public class BidController : CustomBaseController
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService) => _bidService = bidService;
        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] BidAllFilterDto filter)
            => CreateActionResult(await _bidService.GetAllAsync(filter));

        [HttpGet("Details/[action]")]
        public async Task<IActionResult> Get([FromQuery] BidDetailsFilterDto filter)
            => CreateActionResult(await _bidService.GetBidDetailsAsync(filter));

        [HttpGet("{bidMainId}")]
        public async Task<IActionResult> GetCard(int bidMainId)
            => CreateActionResult(await _bidService.GetMainLoadAsync(bidMainId));

        //[HttpGet("[action]/{bidMainId}")]
        //public async Task<IActionResult> GetCardWithLists(int bidMainId)
        //    => CreateActionResult(await _bidService.GetBidCardAsync(bidMainId));

        [HttpPost]
        public async Task<IActionResult> Save(BidMainDto bidMain)
           => CreateActionResult(await _bidService.SaveBidMainAsync(bidMain));
    }
}
