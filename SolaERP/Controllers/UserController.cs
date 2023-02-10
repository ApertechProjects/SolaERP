using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetUserByToken([FromHeader] string authToken)
            => CreateActionResult(await _userService.GetUserByTokenAsync(authToken));

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDto dto)
            => CreateActionResult(await _userService.UpdateAsync(dto));

        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromHeader] string authToken)
            => CreateActionResult(await _userService.RemoveUserByTokenAsync(authToken));

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
            => CreateActionResult(await _userService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> GetActiveUsersAsync()
            => CreateActionResult(await _userService.GetActiveUsersAsync());


    }
}
