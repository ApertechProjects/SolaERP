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

      
        [HttpGet]
        public async Task<IActionResult> GetAccountCodeList()
          => CreateActionResult(await _accountCodeService.GetAllAsync());

   
        [HttpGet("{businessUnitCode}")]
        public async Task<IActionResult> GetAccountCodes(string businessUnitCode)
          => CreateActionResult(await _accountCodeService.GetAccountCodesByBusinessUnit(businessUnitCode));

    }
}
