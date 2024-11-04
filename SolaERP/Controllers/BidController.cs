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

        [HttpGet("[action]/{businessUnitId}")]
        public async Task<IActionResult> GetRFQList(int businessUnitId)
            => CreateActionResult(await _bidService.GetRfqListAsync(User.Identity.Name, businessUnitId));

      

      
    }
}