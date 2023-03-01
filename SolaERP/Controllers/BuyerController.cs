using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BuyerController : CustomBaseController
    {
        private readonly IBuyerService _buyerService;
        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBuyersByTokenAsync([FromHeader] string authToken)
        => CreateActionResult(await _buyerService.GetBuyerByUserTokenAsync(authToken));
    }
}
