using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CurrencyController : CustomBaseController
    {
        private readonly ICurrencyCodeService _currencyService;
        public CurrencyController(ICurrencyCodeService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        ///Retrieves a list of all available currency codes.
        /// </summary>
        /// <remarks>The GetCurrencyCodes endpoint provided by the Currency controller retrieves a list of all available currency codes</remarks>
        ///<param name="authToken">The token used to authenticate the user who performs the operation</param>
        ///<param name="businessUnitCode">The unique identifier of the business unit for which to retrieve currency codes.</param>
        [HttpGet("{businessUnitCode}")]
        public async Task<IActionResult> GetCurrencyCodesByBusinessUnitCode(string businessUnitCode)
            => CreateActionResult(await _currencyService.GetCurrencyCodesByBusinessUnitCode(businessUnitCode));
    }
}
