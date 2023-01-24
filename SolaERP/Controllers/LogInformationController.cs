using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.LogInfo;

namespace SolaERP.Controllers
{
    [ApiController]
    [Authorize]
    public class LogInformationController : CustomBaseController
    {
        private readonly ILogInformationService _logInformationService;
        public LogInformationController(ILogInformationService logInformationService)
        {
            _logInformationService = logInformationService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllLogInformationsAsync(LogInfoGetDto getParameters)
            => CreateActionResult(await _logInformationService.GetAllLogInformationAsync(getParameters));

        [HttpPost]
        public async Task<IActionResult> GetSingleLogInformationsAsync(LogInfoGetDto getParameters)
            => CreateActionResult(await _logInformationService.GetSingleLogInformationAsync(getParameters));
    }
}
