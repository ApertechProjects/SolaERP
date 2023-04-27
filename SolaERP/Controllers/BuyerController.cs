using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BuyerController : CustomBaseController
    {
        private readonly IBuyerService _buyerService;
        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }


        [HttpGet("{businessUnitCode}")]
        public async Task<IActionResult> GetBuyersAsync(string businessUnitCode)
            => CreateActionResult(await _buyerService.GetBuyersAsync(User.Identity.Name, businessUnitCode));

    }
}
