using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("{id}")]
        public async Task<UserDto> GetUserById(int id)
        {
            return await _userService.GetByUserId(id);
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> UpdateUser(UserUpdateDto dto)
        {
            return await _userService.UpdateUserAsync(dto);
        }

        [HttpDelete]
        public async Task<ApiResponse<bool>> RemoveUser(int Id)
        {
            return await _userService.RemoveAsync(Id);
        }

        [HttpGet]
        public async Task<ApiResponse<List<UserDto>>> GetAllUsers()
        {
            return await _userService.GetAllAsync();
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenus()
        //{
        //    return await _userService.GetUserMenusAsync();
        //}

    }
}
