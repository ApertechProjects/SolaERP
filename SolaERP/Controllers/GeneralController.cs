using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GeneralController : CustomBaseController
    {
        private readonly IGeneralService _generalService;

        public GeneralController(IGeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet]
        public IActionResult CheckAuthorize()
              => CreateActionResult(ApiResponse<NoContentDto>.Success(200));

        [HttpGet]
        public async Task<IActionResult> RejectReasonsAsync()
            => CreateActionResult(await _generalService.RejectReasons());

        [HttpGet]
        public async Task<IActionResult> RejectReasonsForInvoiceAsync()
          => CreateActionResult(await _generalService.RejectReasonsForInvoice());


        [HttpGet]
        public async Task<IActionResult> GetStatusAsync()
            => CreateActionResult(await _generalService.GetStatus());

        [HttpGet]
        public async Task<IActionResult> BusinessCategories()
            => CreateActionResult(await _generalService.BusinessCategories());

        [HttpGet]
        public async Task<IActionResult> GetBaseAndReportCurrencyRate(
            [BindRequired] DateTime date,
            [BindRequired] string currency,
            [BindRequired] int businessUnitId) =>
            CreateActionResult(await _generalService.GetBaseAndReportCurrencyRateAsync(date, currency, businessUnitId));

        [HttpGet]
        public async Task<IActionResult> GetBaseAndReportCurrencyRates(
        [BindRequired] DateTime date,
        [BindRequired] int businessUnitId) =>
        CreateActionResult(await _generalService.GetBaseAndReportCurrencyRatesAsync(date, businessUnitId));
    }

}