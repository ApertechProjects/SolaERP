using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
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


        [HttpGet]
        public async Task<ApiResponse<UserDto>> GetUserByToken([FromHeader] string authToken)
        {
            return await _userService.GetUserByTokenAsync(authToken);
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> UpdateUser(UserDto dto)
        {
            return await _userService.UpdateAsync(dto);
        }

        [HttpDelete]
        public async Task<ApiResponse<bool>> RemoveUser([FromHeader] string authToken)
        {
            return await _userService.RemoveUserByTokenAsync(authToken);
        }

        [HttpGet]
        public async Task<ApiResponse<List<UserDto>>> GetAllUsers()
        {
            return await _userService.GetAllAsync();
        }

        //[HttpGet("{groupId}")]
        //public async Task<ApiResponse<UserDto>> GetUsersForGroup(int groupId)
        //{
        //    return _userService.Get
        //}
    }
}
