using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Controllers;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Layout;
using SolaERP.Infrastructure.Models;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LayoutController : CustomBaseController
    {
        private readonly ILayoutService _layoutService;

        public LayoutController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetUserLayout(string key)
            => CreateActionResult(await _layoutService.GetUserLayoutAsync(User.Identity.Name, key));

        [HttpPost]
        public async Task<IActionResult> SaveLayout(LayoutDto layout)
            => CreateActionResult(await _layoutService.SaveLayoutAsync(User.Identity.Name, layout));

        [HttpDelete]
        public async Task<IActionResult> DeleteLayout(string key)
            => CreateActionResult(await _layoutService.DeleteLayoutAsync(User.Identity.Name, key));
    }
}
