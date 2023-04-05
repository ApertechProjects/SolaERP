using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [Authorize]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        public UserController(IUserService userService, IVendorService vendorService)
        {
            _userService = userService;
            _vendorService = vendorService;
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

        [HttpGet]
        public async Task<IActionResult> GetActiveUsersWithoutCurrentUserAsync([FromHeader] string authToken)
          => CreateActionResult(await _userService.GetActiveUsersWithoutCurrentUserAsync(authToken));

        [HttpPost]
        public async Task<IActionResult> GetUserWFAAsync([FromHeader] string authToken, UserWFAGetRequest model)
            => CreateActionResult(await _userService.GetUserWFAAsync(authToken, model));

        [HttpPost]
        public async Task<IActionResult> GetUserAllAsync([FromHeader] string authToken, UserAllQueryRequest model)
            => CreateActionResult(await _userService.GetUserAllAsync(authToken, model));

        [HttpPost]
        public async Task<IActionResult> GetUserCompanyAsync([FromHeader] string authToken, List<int> userStatus, bool allStatus = false)
            => CreateActionResult(await _userService.GetUserCompanyAsync(authToken, userStatus, allStatus));

        [HttpPost]
        public async Task<IActionResult> GetUserVendorAsync([FromHeader] string authToken, List<int> userStatus, bool allStatus = false)
            => CreateActionResult(await _userService.GetUserVendorAsync(authToken, userStatus, allStatus));

        [HttpPost]
        public async Task<IActionResult> UserChangeStatusAsync([FromHeader] string authToken, UserChangeStatusModel model)
            => CreateActionResult(await _userService.UserChangeStatusAsync(authToken, model));

        [HttpPost]
        public async Task<IActionResult> SaveUser(User user)
        {
            var result = await _userService.SaveUserAsync(user);

            if (result)
                return Ok(ApiResponse<NoContentDto>.Success(204));

            return BadRequest(result);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
            => CreateActionResult(await _userService.GetUserInfo(userId));

        [HttpGet]
        public async Task<IActionResult> GetERPUser()
            => CreateActionResult(await _userService.GetERPUser());

    }
}
