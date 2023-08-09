using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Models;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : CustomBaseController
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService) => _bidService = bidService;
        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] BidAllFilterDto filter)
            => CreateActionResult(await _bidService.GetAllAsync(filter));

        [HttpGet("[action]/{bidMaind}")]
        public async Task<IActionResult> GetAll([FromHeader] int bidMainId)
            => CreateActionResult(await _bidService.GetMainLoadAsync(bidMainId));

        [HttpPost]
        public async Task<IActionResult> Save(BidMainDto bidMain)
           => CreateActionResult(await _bidService.SaveBidMainAsync(bidMain));
    }
}
