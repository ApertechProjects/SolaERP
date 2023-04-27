using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountCodeController : CustomBaseController
    {
        private readonly IAccountCodeService _accountCodeService;
        public AccountCodeController(IAccountCodeService accountCodeService)
        {
            _accountCodeService = accountCodeService;
        }

        /// <summary>
        /// Get a list of all account codes
        /// </summary>
        /// <remarks>This endpoint returns a list of all account codes in the accounting system.</remarks>
        [HttpGet]
        public async Task<IActionResult> GetAccountCodeList()
          => CreateActionResult(await _accountCodeService.GetAllAsync());

        /// <summary>
        ///Get a list of account codes for a specific business unit
        /// </summary>
        /// <remarks>This endpoint returns a list of account codes that are associated with a specific business unit in the accounting system.</remarks>
        /// <param name="businessUnit">The unique identifier of the business unit for which to retrieve account codes.</param>
        [HttpGet]
        public async Task<IActionResult> GetAccountCodesByBusinessUnitId(string businessUnit)
          => CreateActionResult(await _accountCodeService.GetAccountCodesByBusinessUnit(businessUnit));

    }
}
