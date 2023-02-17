using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UOMController : CustomBaseController
    {
        private readonly IUOMService _uomService;
        public UOMController(IUOMService uomService)
        {
            _uomService = uomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUOMList()
            => CreateActionResult(await _uomService.GetAllAsync());
    }
}
