using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<UserDto> GetUserById()
        {
            return await _userService.GetByUserId(Kernel.CurrentUserId);
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> UpdateUser(UserUpdateDto dto)
        {
            return await _userService.UpdateUserAsync(dto);
        }

        [HttpDelete]
        public async Task<ApiResponse<bool>> RemoveUser(UserDto dto)
        {
            return await _userService.RemoveAsync(dto);
        }

        [HttpGet]
        public async Task<ApiResponse<List<UserDto>>> GetAllUsers()
        {
            return await _userService.GetAllAsync();
        }

    }
}
