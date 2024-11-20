using AngleSharp.Dom;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SolaERP.Application.Contracts.Services;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class AccountCodeController : CustomBaseController
    {
        private readonly IAccountCodeService _accountCodeService;
        public AccountCodeController(IAccountCodeService accountCodeService)
        {
            _accountCodeService = accountCodeService;
        }

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> GetAccountCodes(int businessUnitId)
          => CreateActionResult(await _accountCodeService.GetAccountCodesByBusinessUnit(businessUnitId));

		[HttpGet("{businessUnitId}/{anlCode}")]
		public async Task<IActionResult> GetAccountCodesByConfig(int businessUnitId, string anlCode)
		  => CreateActionResult(await _accountCodeService.GetAccountCodesByBusinessUnitAndAnlCode(businessUnitId, anlCode));



	}
}
