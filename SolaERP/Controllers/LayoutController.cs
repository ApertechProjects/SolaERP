using Microsoft.AspNetCore.Mvc;
using SolaERP.Controllers;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Layout;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LayoutController : CustomBaseController
    {
        private readonly ILayoutService _layoutService;

        public LayoutController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        [HttpGet("{layoutKey}")]
        public async Task<IActionResult> GetUserLayout([FromHeader] string authToken, string layoutKey)
            => CreateActionResult(await _layoutService.GetUserLayoutAsync(authToken, layoutKey));

        [HttpPost]
        public async Task<IActionResult> SaveLayout([FromHeader] string authToken, LayoutDto layout)
            => CreateActionResult(await _layoutService.SaveLayoutAsync(authToken, layout));

        [HttpDelete]
        public async Task<IActionResult> DeleteLayout([FromHeader] string authToken, LayoutDto layout)
            => CreateActionResult(await _layoutService.DeleteLayoutAsync(authToken, layout));
    }
}
