using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Controllers;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Language;
using SolaERP.Infrastructure.Dtos.Translate;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
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
        public async Task<IActionResult> GetLanguagesLoadAsync()
            => CreateActionResult(await _languageService.GetLanguagesLoadAsync());

        [HttpGet("{code}")]
        public async Task<IActionResult> GetTranslatesLoadByLanguageCodeAsync(string code)
          => CreateActionResult(await _languageService.GetTranslatesLoadByLanguageCodeAsync(code));

        [HttpPost]
        public async Task<IActionResult> SaveLanguageAsync(LanguageDto language)
            => CreateActionResult(await _languageService.SaveLanguageAsync(language));

        [HttpPost]
        public async Task<IActionResult> SaveTranslateAsync(TranslateDto translate)
         => CreateActionResult(await _languageService.SaveTranslateAsync(translate));

        [HttpPost]
        public async Task<IActionResult> DeleteLanguageAsync(int id)
           => CreateActionResult(await _languageService.DeleteLanguageAsync(id));

        [HttpPost]
        public async Task<IActionResult> DeleteTranslateAsync(int id)
        => CreateActionResult(await _languageService.DeleteTranslateAsync(id));
    }
}
