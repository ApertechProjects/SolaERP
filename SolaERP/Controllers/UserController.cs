using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
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

        [HttpGet]
        public async Task<string> GetUserNameByToken([FromHeader] string authToken)
          => await _userService.GetUserNameByTokenAsync(authToken);

        /// <summary>
        ///Updates the given user by id
        /// </summary>
        /// <remarks>Users who are authenticated and authorized to perform the action will be able to access the endpoint.</remarks>
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDto dto)
            => CreateActionResult(await _userService.UpdateAsync(dto));

        /// <summary>
        ///Removes the user which referenced with authToken
        /// </summary>
        /// <remarks>Users who are authenticated and authorized to perform the action will be able to access the</remarks>
        /// <param name="authToken">userIdentifier which returns in Login or Register.</param>
        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromHeader] string authToken)
            => CreateActionResult(await _userService.RemoveUserByTokenAsync(authToken));

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
            => CreateActionResult(await _userService.GetAllAsync());

        /// <summary>
        ///Gets all active users for user list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveUsersAsync()
            => CreateActionResult(await _userService.GetActiveUsersAsync());




    }
}
