using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;
        public AccountController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public ApiResponse<List<UserDto>> GetAllUsers()
        {
            return _userService.GetAll();
        }

        [HttpGet]
        public async Task<ApiResponse<Token>> Login([FromQuery] LoginRequestDto dto)
        {
            return await _userService.LoginAsync(dto);
        }

        [HttpPost]
        public ApiResponse<bool> AddUser(UserDto dto)
        {
            return _userService.Register(dto);
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> UpdateUser(UserDto dto)
        {
            return await _userService.UpdateUser(dto);
        }

        [HttpDelete]
        public ApiResponse<bool> RemoveUser(UserDto dto)
        {
            return _userService.RemoveUser(dto);
        }
    }
}
