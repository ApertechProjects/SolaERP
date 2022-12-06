using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }


        [HttpGet]
        public async Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChilds()
        {
            return await _menuService.GetUserMenusWithChildsAsync();
        }
    }
}
