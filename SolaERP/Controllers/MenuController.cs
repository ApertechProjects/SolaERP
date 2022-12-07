using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }


        [HttpGet("{token}")]
        public async Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChilds(string token)
        {
            return await _menuService.GetUserMenusWithChildsAsync(token);
        }

        [HttpGet("{token}")]
        public async Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilages(string token)
        {
            return await _menuService.GetUserMenusWithPrivilagesAsync(token);
        }
    }
}
