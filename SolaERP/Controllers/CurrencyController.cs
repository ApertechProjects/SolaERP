using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CurrencyController : CustomBaseController
    {
        private readonly ICurrencyCodeService _currencyService;
        public CurrencyController(ICurrencyCodeService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencyCodesAsync()
            => CreateActionResult(await _currencyService.GetAllAsync());
    }
}
