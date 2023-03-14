using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LanguageController : CustomBaseController
    {
        private readonly ILanguageService _languageService;
        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguagesLoad()
            => CreateActionResult(await _languageService.GetLanguagesLoad());

        [HttpGet("{code}")]
        public async Task<IActionResult> GetTranslatesLoadByLanguageCode(string code)
          => CreateActionResult(await _languageService.GetTranslatesLoadByLanguageCode(code));
    }
}
